using Otus.Teaching.PromoCodeFactory.DataAccess;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Extensions
{
    public class UseDataContextAttribute : UseDbContextAttribute
    {
        public UseDataContextAttribute() : base(typeof(DataContext)) { }
    }
}
