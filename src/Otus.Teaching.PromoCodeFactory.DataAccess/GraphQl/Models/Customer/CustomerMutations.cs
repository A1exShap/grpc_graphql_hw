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
    }
}
