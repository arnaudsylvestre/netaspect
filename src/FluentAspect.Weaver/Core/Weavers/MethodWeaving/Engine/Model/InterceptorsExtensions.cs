using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model
{
    public static class InterceptorsExtensions
    {
        public static bool HasBefore(this List<MethodWeavingConfiguration> interceptors)
        {
            return interceptors.Any(interceptor_L => interceptor_L.Before.Method != null);
        }

        public static bool HasAfter(this List<MethodWeavingConfiguration> interceptors)
        {
            return interceptors.Any(interceptor_L => interceptor_L.After.Method != null);
        }

        public static bool HasOnException(this List<MethodWeavingConfiguration> interceptors)
        {
            return interceptors.Any(interceptor_L => interceptor_L.OnException.Method != null);
        }

        public static bool HasOnFinally(this List<MethodWeavingConfiguration> interceptors)
        {
            return interceptors.Any(interceptor_L => interceptor_L.OnFinally.Method != null);
        }
    }
}