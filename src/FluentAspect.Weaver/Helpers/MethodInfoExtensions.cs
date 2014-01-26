using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using ICustomAttributeProvider = System.Reflection.ICustomAttributeProvider;
using MonoCustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodInfoExtensions
    {
        public static IEnumerable<NetAspectDefinition> GetNetAspectAttributes(this ICustomAttributeProvider method)
        {
            return method.GetCustomAttributes(true).Select(a => new NetAspectDefinition(a)).Where(m => m.IsValid);
        }
        
    }
}