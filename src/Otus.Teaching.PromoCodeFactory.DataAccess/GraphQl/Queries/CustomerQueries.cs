using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Extensions;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Queries
{
    [ExtendObjectType(Name = "Query")]
    public class CustomerQueries
    {
        [UseDataContext]
        public Task<List<Customer>> GetCustomersAsync(
            [ScopedService] DataContext context)
        {
            return context.Customers.ToListAsync();
        }

        // TODO try to use dataloader
        [UseDataContext]
        public Task<Customer> GetCustomerAsync(
             Guid id,
            [ScopedService] DataContext context
            )
        {
            return context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
