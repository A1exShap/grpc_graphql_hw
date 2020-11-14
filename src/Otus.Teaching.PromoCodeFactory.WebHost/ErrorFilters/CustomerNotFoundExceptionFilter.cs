using HotChocolate;
using Otus.Teaching.PromoCodeFactory.Core.Exceptions;

namespace Otus.Teaching.PromoCodeFactory.WebHost.ErrorFilters
{
    class CustomerNotFoundExceptionFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Exception is CustomerNotFoundException ex)
                return error.WithMessage($"Customer with id {ex.CustomerId} not found");

            return error;
        }
    }
}
