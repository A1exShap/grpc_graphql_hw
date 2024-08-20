using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Otus.Teaching.PromoCodeFactory.GraphQL.DataLoaders;
using Otus.Teaching.PromoCodeFactory.GraphQL.Extensions;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Customers
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class CustomerQueries
    {
        [UseDataContext]
        [UsePaging]
        public IQueryable<Customer> GetCustomers([ScopedService] DataContext context)
            => context.Customers.OrderBy(context => context.LastName);

        public Task<Customer> GetCustomerByIdAsync(
            [ID(nameof(Customer))] Guid id,
            CustomerByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
            => dataLoader.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Customer>> GetCustomersByIdAsync(
            [ID(nameof(Customer))] Guid[] ids,
            CustomerByIdDataLoader dataLoader,
            CancellationToken cancellationToken)
            => await dataLoader.LoadAsync(ids, cancellationToken);
    }
}
