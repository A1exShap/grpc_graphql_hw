using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Core;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Models.Customer
{
    public class DeleteCustomerPayload : Payload
    {
        public Guid Id { get; }
        public DeleteCustomerPayload(Guid id)
        {
            Id = id;
        }

        public DeleteCustomerPayload(UserError error) : base(new[] { error })
        {
        }
    }
}
