using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodInfoExtensions
    {
        //public static List<NetAspectAttribute> GetNetAspectAttributes(this ICustomAttributeProvider method, bool inherit)
        //{
        //    IEnumerable<NetAspectAttribute> customAttributes_L =
        //        method.GetCustomAttributes(inherit).Select((a) => new NetAspectAttribute(a));
        //    return (from m in customAttributes_L where m.IsValid select m).ToList();
        //}
        public static List<MethodWeavingConfiguration> GetMethodWeavingAspectAttributes(this ICustomAttributeProvider method, bool inherit)
        {
            IEnumerable<NetAspectAttribute> customAttributes_L =
                method.GetCustomAttributes(inherit).Select((a) => new NetAspectAttribute(a));
            return (from m in customAttributes_L where m.IsValid && m.Kind == NetAspectAttributeKind.MethodWeaving select m.MethodWeavingConfiguration).ToList();
        }
        public static List<CallWeavingConfiguration> GetCallWeavingAspectAttributes(this ICustomAttributeProvider method, bool inherit)
        {
            IEnumerable<NetAspectAttribute> customAttributes_L =
                method.GetCustomAttributes(inherit).Select((a) => new NetAspectAttribute(a));
            return (from m in customAttributes_L where m.IsValid && m.Kind == NetAspectAttributeKind.CallWeaving select m.CallWeavingConfiguration).ToList();
        }
    }
}