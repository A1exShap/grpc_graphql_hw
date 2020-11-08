using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Types
{
    public class CustomerType : ObjectType<Customer>
    {
        protected override void Configure(IObjectTypeDescriptor<Customer> descriptor)
        {
            base.Configure(descriptor);
            
            descriptor.Name("Customer");

            //descriptor.Field(f => f.Email)
            //    .Type<StringType>();

            //descriptor.Field(f => f.FirstName)
            //    .Type<StringType>();

            //descriptor.Field(f => f.LastName)
            //    .Type<StringType>();

            //descriptor.Field(f => f.FullName)
            //    .Type<StringType>();

            //descriptor.Field(f => f.Id)
            //    .Type<StringType>();

            descriptor.Field(f => f.Preferences)
                .UsePaging<ListType<CustomerPreferenceType>>();
        }
    }
}
