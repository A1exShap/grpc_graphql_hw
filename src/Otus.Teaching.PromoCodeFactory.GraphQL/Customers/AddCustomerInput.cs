using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Customers
{
    public record AddCustomerInput
    (
        string FirstName,
        string LastName,
        string Email,
        [property: ID(nameof(Customer))]
        IReadOnlyList<Guid> PreferencesIds
    );
}
