using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Types
{
    public class CustomerPreferenceType : ObjectType<CustomerPreference>
    {
        protected override void Configure(IObjectTypeDescriptor<CustomerPreference> descriptor)
        {
            //base.Configure(descriptor);

            descriptor.Name("CustomerPreference");
            
            descriptor.Field(f => f.Customer)
                .UsePaging<CustomerType>();

            descriptor.Field(f => f.Preference)
                .UsePaging<PreferenceType>();
        }
    }
}
