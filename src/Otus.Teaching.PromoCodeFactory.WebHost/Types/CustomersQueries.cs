using HotChocolate;
using HotChocolate.Types;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Types
{
    [ExtendObjectType(Name = "Query")]
    public class CustomersQueries
    {
        [UseFiltering]
        public async Task<IEnumerable<Customer>> GetCustomersAsync([Service] IRepository<Customer> customerRepository)
        {
            return await customerRepository.GetAllAsync();
        }
    }
}
