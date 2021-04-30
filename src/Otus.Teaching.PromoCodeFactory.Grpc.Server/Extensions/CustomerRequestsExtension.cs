using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using CustomerEntity = Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer;

namespace Otus.Teaching.PromoCodeFactory.Grpc.Server.Extensions
{
    public static class CustomerRequestsExtension
    {
        public static CustomerEntity MapCustomer(this AddCustomerRequest request, IEnumerable<Preference> preferences)
        {
            var customer = new CustomerEntity
            {
                Id = Guid.NewGuid()
            };
            
            return Map(request, preferences, customer);
        }

        public static CustomerEntity MapCustomer(this EditCustomerRequest request, IEnumerable<Preference> preferences, CustomerEntity customer)
        {
            return Map(request, preferences, customer);
        }

        private static CustomerEntity Map(IMessage request, IEnumerable<Preference> preferences, CustomerEntity customer)
        {
            switch (request)
            {
                case AddCustomerRequest addRequest:
                    customer.MapFrom(addRequest);
                    break;
                case EditCustomerRequest editRequest:
                    customer.MapFrom(editRequest);
                    break;
            }

            if (preferences != null)
                customer.Preferences = preferences.Select(x => new CustomerPreference()
                {
                    CustomerId = customer.Id,
                    Preference = x,
                    PreferenceId = x.Id
                }).ToList();
            
            return customer;
        }

        private static void MapFrom(this CustomerEntity customer, AddCustomerRequest request)
        {
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
        }
        private static void MapFrom(this CustomerEntity customer, EditCustomerRequest request)
        {
            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;
        }
    }
}