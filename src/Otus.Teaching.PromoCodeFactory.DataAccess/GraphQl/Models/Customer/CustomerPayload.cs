using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Core;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Models.Customer
{
    public class CustomerPayload : Payload
    {
        public PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer Customer { get; }

        public CustomerPayload(PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer customer)
        {
            Customer = customer;
        }

        public CustomerPayload(UserError error) : base(new[] { error })
        {
        }
    }
}
