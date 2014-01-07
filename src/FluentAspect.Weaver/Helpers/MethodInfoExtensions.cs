using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodInfoExtensions
    {
        public static IEnumerable<MethodWeavingConfiguration> GetMethodWeavingAspectAttributes(this ICustomAttributeProvider method)
        {
            return method.GetCustomAttributes(true).Select((a) => new MethodWeavingConfiguration(a)).Where(m => m.IsValid);
        }
        public static IEnumerable<CallWeavingConfiguration> GetCallWeavingAspectAttributes(this ICustomAttributeProvider method)
        {
            return method.GetCustomAttributes(true).Select((a) => new CallWeavingConfiguration(a)).Where(m => m.IsValid);
        }
        
    }
}