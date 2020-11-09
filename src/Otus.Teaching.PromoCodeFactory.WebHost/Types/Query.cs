using HotChocolate;
using HotChocolate.Types;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Types
{
    public class Query
    {
        [UseSelection]
        [UseFiltering]
        public async Task<IQueryable<Customer>> GetCustomersAsync([Service] IRepository<Customer> customerRepository)
        {
            return (await customerRepository.GetAllAsync()).AsQueryable();
        }
    }
}
