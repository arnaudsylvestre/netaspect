using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers.IL;

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
   }

    public class MethodToWeave
    {
        private readonly List<MethodWeavingConfiguration> interceptors;
        private readonly Method method;

        public MethodToWeave(List<MethodWeavingConfiguration> interceptors, Method method)
        {
            this.interceptors = interceptors;
            this.method = method;
        }

        public Method Method
        {
            get { return method; }
        }

        public List<MethodWeavingConfiguration> Interceptors
        {
            get { return interceptors; }
        }
    }
}