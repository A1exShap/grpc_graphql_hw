using Microsoft.AspNetCore.Mvc;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.WebHost.Mappers;
using Otus.Teaching.PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Types
{
    public class Mutation
    {
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Preference> preferenceRepository;
        private readonly IRepository<CustomerPreference> customerPreferenceRepository;

        public Mutation(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository,
                        IRepository<CustomerPreference> customerPreferenceRepository)
        {
            this.customerRepository = customerRepository;
            this.preferenceRepository = preferenceRepository;
            this.customerPreferenceRepository = customerPreferenceRepository;
        }

        public async Task<Customer> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await preferenceRepository
                .GetRangeByIdsAsync(request.PreferenceIds);
            Customer customer = CustomerMapper.MapFromModel(request, preferences);
            await customerRepository.AddAsync(customer);
            return customer;
        }

        public async Task<Customer> DeleteCustomerAsync(Guid id)
        {
            var customer = await customerRepository.GetByIdAsync(id);

            //if (customer == null)

            await customerRepository.DeleteAsync(customer);
            return customer;
        }

        public async Task<Customer> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await customerRepository.GetByIdAsync(id);
            //if (customer == null)

            var preferences = await preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);
            var oldCustomerPreferences = customerPreferenceRepository.GetAllAsync().Result.Where(x => x.CustomerId == id);
            await customerPreferenceRepository.DeleteManyAsync(oldCustomerPreferences);
            CustomerMapper.MapFromModel(request, preferences, customer);
            await customerRepository.UpdateAsync(customer);
            return customer;
        }
    }
}
