using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.DataLoaders
{
    public class PreferenceByCustomerIdDataLoader : GroupedDataLoader<Guid, Preference>
    {
        private readonly IDbContextFactory<DataContext> _dataContextFactory;

        public PreferenceByCustomerIdDataLoader(IDbContextFactory<DataContext> dataContextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions options)
            : base(batchScheduler, options)
        {
            _dataContextFactory = dataContextFactory ??
                throw new ArgumentNullException(nameof(dataContextFactory));
        }

        protected override async Task<ILookup<Guid, Preference>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
        {
            await using DataContext dataContext =
                _dataContextFactory.CreateDbContext();

            var preferences = await dataContext.Customers
                .Where(c => keys.Contains(c.Id))
                .Include(c => c.Preferences)
                .SelectMany(c => c.Preferences)
                .Include(p => p.Preference)
                .ToListAsync(cancellationToken);

            return preferences.ToLookup(p => p.CustomerId, p => p.Preference);
        }
    }
}
