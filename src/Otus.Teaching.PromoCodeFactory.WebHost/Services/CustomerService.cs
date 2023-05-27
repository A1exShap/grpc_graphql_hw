using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomerService : Customers.CustomersBase
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomerService
        (
            ILogger<CustomerService> logger,
            IRepository<Customer> customerRepository,
            IRepository<Preference> preferenceRepository
        )
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        public override async Task<CustomerListReply> GetCustomers(CustomerListRequest request, ServerCallContext context)
        {
            var customers = await _customerRepository.GetAllAsync();

            return customers.ToCustomerListReply();
        }

        public override async Task<CustomerReply> GetCustomer(CustomerRequest request, ServerCallContext context)
        {
            var customer = await _customerRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (customer == null)
            {
                var message = $"Customer (${request.Id}) is not found.";
                throw new RpcException(new Status(StatusCode.NotFound, message));
            }
            return (CustomerReply)customer;
        }

        public override async Task<CustomerReply> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(
                request.PreferenceIds.Select(x => Guid.Parse(x)).ToList());

            var customer = new Customer
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };

            customer.Preferences = preferences.Select(p => new CustomerPreference()
            {
                Customer = customer,
                Preference = p,
            }).ToList();

            await _customerRepository.AddAsync(customer);

            return (CustomerReply)customer;
        }

        public override async Task<Empty> EditCustomer(EditCustomerRequest request, ServerCallContext context)
        {
            var customer = await _customerRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (customer == null)
            {
                var message = $"Customer (${request.Id}) is not found.";
                throw new RpcException(new Status(StatusCode.NotFound, message));
            }

            var data = request.Data;
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(
                data.PreferenceIds.Select(x => Guid.Parse(x)).ToList());

            if (!string.IsNullOrEmpty(data.Email)) customer.Email = data.Email;
            if (!string.IsNullOrEmpty(data.FirstName)) customer.FirstName = data.FirstName;
            if (!string.IsNullOrEmpty(data.LastName)) customer.LastName = data.LastName;

            customer.Preferences = customer.Preferences
                .Union(preferences
                    .Where(p => !customer.Preferences.Any(cp => cp.PreferenceId == p.Id))
                    .Select(p => new CustomerPreference()
                    {
                        Customer = customer,
                        Preference = p,
                    })).ToList();

            await _customerRepository.UpdateAsync(customer);

            return new Empty();
        }

        public override async Task<Empty> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
        {
            var customer = await _customerRepository.GetByIdAsync(Guid.Parse(request.Id));
            var httpContext = context.GetHttpContext();

            if (customer == null)
            {
                var message = $"Customer (${request.Id}) is not found.";
                throw new RpcException(new Status(StatusCode.NotFound, message));
            }

            await _customerRepository.DeleteAsync(customer);

            return new Empty();
        }
    }
}
