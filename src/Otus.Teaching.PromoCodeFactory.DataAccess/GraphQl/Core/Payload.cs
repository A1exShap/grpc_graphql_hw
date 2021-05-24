using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Core
{
    public abstract class Payload
    {
        public IReadOnlyList<UserError> Errors { get; }

        protected Payload()
        {
            Errors = null;
        }
        protected Payload(IReadOnlyList<UserError> errors)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
        }
    }
}
