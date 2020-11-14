using System;

namespace Otus.Teaching.PromoCodeFactory.Core.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public Guid CustomerId { get; set; }
    }
}
