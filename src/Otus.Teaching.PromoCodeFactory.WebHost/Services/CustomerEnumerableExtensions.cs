using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Linq;

namespace GrpcServer
{
    public static class CustomerEnumerableExtensions
    {
        public static CustomerListReply ToCustomerListReply(this IEnumerable<Customer> customer)
        {
            var list = new CustomerListReply();

            list.Customers.AddRange(customer.Select(x => new CustomerListItem
            {
                Id = x.Id.ToString(),
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName
            }));

            return list;
        }
    }
}
