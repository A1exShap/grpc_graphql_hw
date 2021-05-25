using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Models.Customer
{
    public record AddCustomerInput(
        string FirstName,
        string LastName,
        string Email,
        // TODO: check correct
        IReadOnlyList<Guid> PreferenceIds
    );
}
