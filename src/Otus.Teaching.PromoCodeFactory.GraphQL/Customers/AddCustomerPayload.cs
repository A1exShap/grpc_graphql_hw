using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.GraphQL.Common;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Customers
{
    public class AddCustomerPayload : Payload
    {
        public Customer? Customer { get; }

        public AddCustomerPayload(UserError userError) : base(new[] { userError }) { }

        public AddCustomerPayload(Customer customer)
        {
            Customer = customer;
        }
    }
}
