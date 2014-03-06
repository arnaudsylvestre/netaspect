using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using MonoCustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodInfoExtensions
    {
        public static IEnumerable<NetAspectDefinition> GetNetAspectAttributes(this ICustomAttributeProvider method)
        {
            return
                method.GetCustomAttributes(true).Select(a => new NetAspectDefinition(a.GetType())).Where(m => m.IsValid);
        }
    }
}