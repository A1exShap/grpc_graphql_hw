using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Linq;

namespace GrpcServer
{
    public partial class CustomerReply
    {
        public static explicit operator CustomerReply(Customer customer)
        {
            var reply = new CustomerReply
            {
                Id = customer.Id.ToString(),
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };

            reply.Preferences.AddRange(customer.Preferences.Select(x => new PreferenceListItem()
            {
                Id = x.PreferenceId.ToString(),
                Name = x.Preference.Name
            }));

            return reply;
        }
    }
}
