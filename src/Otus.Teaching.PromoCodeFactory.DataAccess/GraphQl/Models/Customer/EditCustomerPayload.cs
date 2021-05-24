using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Core;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Models.Customer
{
    public class EditCustomerPayload: CustomerPayload
    {
        public EditCustomerPayload(PromoCodeFactory.Core.Domain.PromoCodeManagement.Customer customer) : base(customer)
        {
        }

        public EditCustomerPayload(UserError error) : base(error)
        {
        }
    }
}
