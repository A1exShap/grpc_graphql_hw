using HotChocolate.Types;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Types
{
    internal class PreferenceType : ObjectType<Preference>
    {
        protected override void Configure(IObjectTypeDescriptor<Preference> descriptor)
        {
            base.Configure(descriptor);
        }
    }
}