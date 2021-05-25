using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Core
{
    public class UserError
    {
        public string Message { get; set; }
        public string Code { get; set; }

        public UserError(string message, string code)
        {
            Message = message;
            Code = code;
        }

    }
}
