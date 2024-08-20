using Grpc.Core;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.gRPC.Mappers;

namespace Otus.Teaching.PromoCodeFactory.gRPC.Services
{
    public class CustomerServiceImpl : CustomerService.CustomerServiceBase
    {
        private readonly ILogger<CustomerServiceImpl> _logger;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomerServiceImpl(ILogger<CustomerServiceImpl> logger, IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        public override async Task GetCustomers(Empty request, IServerStreamWriter<CustomerShortResponse> responseStream, ServerCallContext context)
        {
            var customers = await _customerRepository.GetAllAsync();
            foreach (var customer in customers)
            {
                var response = new CustomerShortResponse
                {
                    Id = customer.Id.ToString(),
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                };
                await responseStream.WriteAsync(response);
            }
        }

        public override async Task<CustomerResponse> GetCustomer(CustomerId request, ServerCallContext context)
        {
            var customer = await _customerRepository.GetByIdAsync(Guid.Parse(request.Id));

            var response = new CustomerResponse
            {
                Id = customer.Id.ToString(),
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Preferences = { customer.Preferences.Select(x => new PreferenceResponse { Id = x.PreferenceId.ToString(), Name = x.Preference.Name }) }
            };
            return response;
        }

        public override async Task<CustomerId> CreateCustomer(CreateOrEditCustomerRequest request, ServerCallContext context)
        {
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds.Select(Guid.Parse).ToList());
            var customer = CustomerMapper.MapFromModel(request, preferences);
            await _customerRepository.AddAsync(customer);
            return new CustomerId { Id = customer.Id.ToString() };
        }

        public override async Task<Empty> EditCustomer(EditCustomerRequest request, ServerCallContext context)
        {
            var customer = await _customerRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (customer == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }
            var preferences = await _preferenceRepository.GetRangeByIdsAsync(request.Customer.PreferenceIds.Select(Guid.Parse).ToList());
            CustomerMapper.MapFromModel(request.Customer, preferences, customer);
            await _customerRepository.UpdateAsync(customer);
            return new Empty();
        }

        public override async Task<Empty> DeleteCustomer(CustomerId request, ServerCallContext context)
        {
            var customer = await _customerRepository.GetByIdAsync(Guid.Parse(request.Id));
            if (customer == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }
            await _customerRepository.DeleteAsync(customer);
            return new Empty();
        }

    }
}
