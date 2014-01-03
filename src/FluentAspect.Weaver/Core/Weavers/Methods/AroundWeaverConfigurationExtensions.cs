using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
   public static class AroundWeaverConfigurationExtensions
   {
      public static bool HasCallOnException(IEnumerable<MethodWeavingConfiguration> interceptorTypes)
      {
         foreach (var interceptorType in interceptorTypes)
         {
            if (interceptorType.OnException.Method != null)
               return true;
         }
         return false;
      }

      public static bool Needs(IEnumerable<MethodWeavingConfiguration> weavingConfigurations, string variableName)
      {
         foreach (var weavingConfiguration_L in weavingConfigurations)
         {
            var parameters = new List<ParameterInfo>();
            var callBefore = weavingConfiguration_L.Before.Method;
            if (callBefore != null)
               parameters.AddRange(callBefore.GetParameters().ToList());
            MethodInfo methodInfo = weavingConfiguration_L.After.Method;
            if (methodInfo != null)
               parameters.AddRange(methodInfo.GetParameters().ToList());
            MethodInfo callOnException = weavingConfiguration_L.OnException.Method;
            if (callOnException != null)
               parameters.AddRange(callOnException.GetParameters().ToList());

            IEnumerable<string> enumerable = from p in parameters where p.Name == variableName select p.Name;
            if (enumerable.Any())
               return true;
         }
         return false;
      }
   }
}