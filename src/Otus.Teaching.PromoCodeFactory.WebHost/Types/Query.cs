using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Types
{
    public class Query
    {
        [UsePaging(SchemaType = typeof(CustomerType))]
        [UseFiltering]
        public async Task<IQueryable<Customer>> GetCustomersAsync([Service] IRepository<Customer> customerRepository)
        {
            return (await customerRepository.GetAllAsync()).AsQueryable();
        }

        [UseFirstOrDefault]
        public IQueryable<Customer> GetCustomerById([Service] DataContext context, Guid id)
        {
            return context.Customers.Where(x => x.Id == id);
        }
    }
}
