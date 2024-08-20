using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using Otus.Teaching.PromoCodeFactory.GraphQL.Common;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Customers
{
    public class ChangeCustomerEmailPayload : Payload
    {
        public Customer? Customer { get; }

        public ChangeCustomerEmailPayload(UserError userError) : base(new[] { userError }) { }

        public ChangeCustomerEmailPayload(Customer customer)
        {
            Customer = customer;
        }
    }
}
