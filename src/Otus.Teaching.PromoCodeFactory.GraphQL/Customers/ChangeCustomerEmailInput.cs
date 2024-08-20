using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.GraphQL.Customers
{
    public record ChangeCustomerEmailInput
    (
        [property: ID(nameof(Customer))]
        string Id,
        string Email
    );
}
