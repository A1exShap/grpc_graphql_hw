using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.DataLoaders
{
    public class CustomerByIdDataLoader : BatchDataLoader<Guid, Customer>
    {
        private readonly IDbContextFactory<DataContext> _dataContextFactory;

        public CustomerByIdDataLoader(IDbContextFactory<DataContext> dataContextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions options)
            : base(batchScheduler, options)
        {
            _dataContextFactory = dataContextFactory ??
                throw new ArgumentNullException(nameof(dataContextFactory));
        }

        protected override async Task<IReadOnlyDictionary<Guid, Customer>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken cancellationToken)
        {
            await using DataContext dataContext =
                _dataContextFactory.CreateDbContext();

            return await dataContext.Customers
                .Where(c => keys.Contains(c.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
