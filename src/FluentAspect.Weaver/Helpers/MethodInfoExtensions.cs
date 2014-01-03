using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core;

namespace FluentAspect.Weaver.Helpers
{
    public static class MethodInfoExtensions
    {
        public static List<NetAspectAttribute> GetNetAspectAttributes(this ICustomAttributeProvider method, bool inherit)
        {
           var customAttributes_L = method.GetCustomAttributes(inherit).Select((a) => new NetAspectAttribute(a));
           return (from m in customAttributes_L where m.IsValid select m).ToList();
        }
    }
}