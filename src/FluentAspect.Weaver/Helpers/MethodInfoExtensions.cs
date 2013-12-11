using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAspect.Weaver.Core.Fluent
{
    public static class MethodInfoExtensions
    {
        public static List<T> GetCustomAttributes<T>(this MemberInfo method, bool inherit)
        {
            return method.GetCustomAttributes(inherit).OfType<T>().ToList();
        }
    }
}