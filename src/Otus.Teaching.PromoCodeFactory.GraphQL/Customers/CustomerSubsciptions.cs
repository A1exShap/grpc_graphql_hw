using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.GraphQL.DataLoaders;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Customers
{
    [ExtendObjectType(OperationTypeNames.Subscription)]
    public class CustomerSubsciptions
    {
        [Subscribe]
        [Topic]
        public Task<Customer> OnCustomerEmailChangedAsync(
            [EventMessage] Guid customerId,
            CustomerByIdDataLoader customerById,
            CancellationToken cancellationToken) 
            => customerById.LoadAsync(customerId, cancellationToken);
    }
}
