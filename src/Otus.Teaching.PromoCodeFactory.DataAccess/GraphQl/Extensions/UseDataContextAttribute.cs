using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.GraphQl.Extensions
{
    class UseDataContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<DataContext>();
        }
    }
}
