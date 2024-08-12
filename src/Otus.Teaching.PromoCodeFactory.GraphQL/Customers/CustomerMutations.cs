using HotChocolate.Subscriptions;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.GraphQL.Common;
using Otus.Teaching.PromoCodeFactory.GraphQL.Extensions;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Customers
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class CustomerMutations
    {
        [UseDataContext]
        public async Task<AddCustomerPayload> AddCustomerAsync(AddCustomerInput input, [ScopedService] DataContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(input.FirstName))
            {
                return new(new UserError("The first name cannot be empty.", "FIRSTNAME_EMPTY"));
            }

            if (string.IsNullOrEmpty(input.LastName))
            {
                return new(new UserError("The last name cannot be empty.", "LASTNAME_EMPTY"));
            }

            if (string.IsNullOrEmpty(input.Email))
            {
                return new(new UserError("Email cannot be empty.", "EMAIL_EMPTY"));
            }

            if (!input.PreferencesIds.Any())
            {
                return new(new UserError("No preference assigned.", "NO_PREFERENCE"));
            }

            var customerPreferences = input.PreferencesIds.Select(id => new CustomerPreference { PreferenceId = id }).ToList();

            var customer = new Customer
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                Preferences = customerPreferences
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync(cancellationToken);

            return new(customer);
        }

        [UseDataContext]
        public async Task<ChangeCustomerEmailPayload> ChangeCustomerEmailAsync(
            ChangeCustomerEmailInput input, 
            [ScopedService] DataContext context,
            [Service]ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(input.Email))
            {
                return new(new UserError("Email cannot be empty.", "EMAIL_EMPTY"));
            }

            if (!Guid.TryParse(input.Id, out var id))
            {
                return new(new UserError("The identifier is in an invalid format.", "ID_INVALID_FORMAT"));
            }

            var customer = await context.Customers.FindAsync(id, cancellationToken);

            if (customer is null)
            {
                return new(new UserError("Customer not found.", "CUSTOMER_NOT_FOUND"));
            }

            customer.Email = input.Email;

            await context.SaveChangesAsync(cancellationToken);

            await eventSender.SendAsync(nameof(CustomerSubsciptions.OnCustomerEmailChangedAsync), customer.Id);

            return new(customer);
        }
    }
}
