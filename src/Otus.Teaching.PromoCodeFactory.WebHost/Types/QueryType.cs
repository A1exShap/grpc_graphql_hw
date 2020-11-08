using HotChocolate.Types;
using Otus.Teaching.PromoCodeFactory.WebHost.RootTypes;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Types
{
    public class QueryType
        : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(t => t.GetCustomersAsync())
                .Type<CustomerType>();

            //descriptor.Field(t => t.GetPreferencesAsync())
            //     .Type<CustomerPreferenceType>();
        }
    }
}
