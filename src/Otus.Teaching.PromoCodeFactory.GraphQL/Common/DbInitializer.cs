using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Common
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IDbContextFactory<DataContext> _dataContextFactory;

        public DbInitializer(IDbContextFactory<DataContext> dataContextFactory)
        {
            _dataContextFactory = dataContextFactory ??
                throw new ArgumentNullException(nameof(dataContextFactory));
        }

        public void InitializeDb()
        {
            using DataContext dataContext =
                _dataContextFactory.CreateDbContext();

            dataContext.Database.EnsureDeleted();
            dataContext.Database.EnsureCreated();

            dataContext.AddRange(FakeDataFactory.Employees);
            dataContext.SaveChanges();

            dataContext.AddRange(FakeDataFactory.Preferences);
            dataContext.SaveChanges();

            dataContext.AddRange(FakeDataFactory.Customers);
            dataContext.SaveChanges();

            dataContext.AddRange(FakeDataFactory.Partners);
            dataContext.SaveChanges();
        }
    }
}
