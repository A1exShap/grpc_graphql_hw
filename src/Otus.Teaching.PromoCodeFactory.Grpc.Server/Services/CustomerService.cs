using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Core;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.Grpc.Server.Extensions;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers;
using CustomerEntity = Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer;

namespace Otus.Teaching.PromoCodeFactory.Grpc.Server.Services
{
    public class CustomerService: Customer.CustomerBase
    {
        private readonly IRepository<CustomerEntity> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomerService(IRepository<CustomerEntity> customerRepository,
            IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        public override async Task<GetCustomersResponse> GetCustomers(GetCustomersRequest request, ServerCallContext context)
        {
            var customers = (await _customerRepository.GetAllAsync()).ToArray();
            var customersList = new RepeatedField<CustomerShort>();
            customersList.AddRange(customers.Select(c => new CustomerShort
            {
                Id = c.Id.ToString(),
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            }));
            
            return new GetCustomersResponse
            {
                Customers = { customersList }
            };
        }

        public override async Task<GetCustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            var customer = await _customerRepository.GetByIdAsync(Guid.Parse(request.Id));

            return new GetCustomerResponse
            {
                Id = customer.Id.ToString(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Preferences = {
                    customer.Preferences
                        .Select(p => new IdName
                        {
                            Id = p.PreferenceId.ToString(), 
                            Name = p.Preference.Name
                        })
                        .ToArray()
                }
            };
        }

        public override async Task<AddCustomerResponse> AddCustomer(AddCustomerRequest request, ServerCallContext context)
        {
            var prefIds = request.PreferenceIds.Select(Guid.Parse).ToList();
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(prefIds);
            var customer = request.MapCustomer(preferences);
            
            var id = await _customerRepository.AddAsync(customer);
            return new AddCustomerResponse
            {
                Id = id.ToString()
            };
        }

        public override async Task<OperationResult> EditCustomer(EditCustomerRequest request, ServerCallContext context)
        {
            var id = Guid.Parse(request.Id);
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return new OperationResult { Success = false, Message = $"Пользователь с id = '{id}' не найден." };

            var prefIds = request.PreferenceIds.Select(Guid.Parse).ToList();
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(prefIds);

            request.MapCustomer(preferences, customer);

            await _customerRepository.UpdateAsync(customer);
            return new OperationResult {Success = true};
        }

        public override async Task<OperationResult> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
        {
            var id = Guid.Parse(request.Id);
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return new OperationResult { Success = false, Message = $"Пользователь с id = '{id}' не найден." };

            await _customerRepository.DeleteAsync(customer);
            return new OperationResult { Success = true };
        }
    }
}