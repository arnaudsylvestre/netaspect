using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodInfoExtensions
    {
        public static List<object> GetNetAspectAttributes(this ICustomAttributeProvider method, bool inherit)
        {
            return (from m in method.GetCustomAttributes(inherit) where m.GetType().GetField("NetAspectAttributeKind", BindingFlags.NonPublic | BindingFlags.Public) != null select m).ToList();
        }
        public static List<Type> GetNetAspectInterceptors(this MemberInfo method)
        {
            return (from m in method.GetNetAspectAttributes(true) select m.GetType()).ToList();
        }
    }
}