using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.GraphQL.DataLoaders;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Customers
{
    [Node]
    [ExtendObjectType(typeof(Customer))]
    public class CustomerNode
    {
        [BindMember(nameof(Customer.Preferences), Replace = true)]
        public Task<Preference[]> GetPreferencesAsync([Parent] Customer customer, PreferenceByCustomerIdDataLoader preferenceLoader, CancellationToken cancellationToken)
            => preferenceLoader.LoadAsync(customer.Id, cancellationToken);

        [NodeResolver]
        public static Task<Customer> GetCustomerByIdAsync(Guid id, CustomerByIdDataLoader customerLoader, CancellationToken cancellationToken)
            => customerLoader.LoadAsync(id, cancellationToken);
    }
}
