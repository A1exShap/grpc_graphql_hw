using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Models.Customer
{
    // TODO: 
    // Вопрос: как нужно создавать класс по изменению сущности?
    // Стоит ли разбивать/добавлять действия по изменению отдельных полей в отдельные методы?
    public record EditCustomerInput
    (
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        IReadOnlyList<Guid> PreferenceIds
    );
}

