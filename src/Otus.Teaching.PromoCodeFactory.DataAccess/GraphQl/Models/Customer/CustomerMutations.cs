using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Core;
using Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Extensions;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Models.Customer
{
    [ExtendObjectType(Name = "Mutation")]
    public class CustomerMutations
    {
        [UseDataContext]
        public async Task<CustomerPayload> AddCustomer(
            AddCustomerInput input,
            [ScopedService] DataContext context,
            CancellationToken token)
        {

            if (input.PreferenceIds.Count == 0)
            {
                return new CustomerPayload(
                    new UserError("No PreferenceIds assigned.", "PreferenceIds"));
            }

            //TODO: add checks
            var customer = new PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer()
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email
            };

            customer.Preferences = new List<CustomerPreference>();
            foreach (var id in input.PreferenceIds)
            {
                customer.Preferences.Add(new CustomerPreference() { PreferenceId = id });
            }

            await context.Customers.AddAsync(customer, token);
            await context.SaveChangesAsync(token);

            return new CustomerPayload(customer);
        }

        [UseDataContext]
        public async Task<EditCustomerPayload> EditCustomer(
            EditCustomerInput input,
            [ScopedService] DataContext context,
            CancellationToken token
        )
        {
            var oldCustomer = context.Customers.FirstOrDefault(x => x.Id == input.Id);
            if (oldCustomer != null)
            {
                oldCustomer.FirstName = input.FirstName;
                oldCustomer.LastName = input.LastName;
                oldCustomer.Email = input.Email;
                if (input.PreferenceIds != null)
                {
                    foreach (var id in input.PreferenceIds)
                    {
                        if (oldCustomer != null)
                        {
                            oldCustomer.Preferences.Clear();
                            oldCustomer.Preferences.Add(new CustomerPreference() { PreferenceId = id });
                        }
                    }

                }

                context.Customers.Update(oldCustomer);
                await context.SaveChangesAsync(token);

                return new EditCustomerPayload(oldCustomer);
            }

            return new EditCustomerPayload(
                new UserError("No PreferenceIds assigned.", "PreferenceIds"));

        }

    }
}
