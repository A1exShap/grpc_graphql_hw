﻿using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.gRPC.Mappers
{
    public class CustomerMapper
    {
        public static Customer MapFromModel(CreateOrEditCustomerRequest model, IEnumerable<Preference> preferences, Customer customer = null)
        {
            if (customer == null)
            {
                customer = new Customer();
                customer.Id = Guid.NewGuid();
            }

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;

            customer.Preferences = preferences.Select(x => new CustomerPreference()
            {
                CustomerId = customer.Id,
                Preference = x,
                PreferenceId = x.Id
            }).ToList();

            return customer;
        }
    }
}