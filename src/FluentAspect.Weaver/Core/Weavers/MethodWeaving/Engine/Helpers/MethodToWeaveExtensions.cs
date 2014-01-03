using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers
{
   public static class MethodToWeaveExtensions
   {
      public static bool HasInterceptorsOnException(this MethodToWeave methodToWeave)
      {
          return methodToWeave.Interceptors.Any(interceptorType => interceptorType.OnException.Method != null);
      }

       public static bool Needs(this MethodToWeave methodToWeave, string variableName)
      {
          foreach (var weavingConfiguration_L in methodToWeave.Interceptors)
         {
            var parameters = new List<ParameterInfo>();
            parameters.AddRange(weavingConfiguration_L.Before.GetParameters());
            parameters.AddRange(weavingConfiguration_L.After.GetParameters());
            parameters.AddRange(weavingConfiguration_L.OnException.GetParameters());
             if ((from p in parameters where p.Name == variableName select p.Name).Any())
               return true;
         }
         return false;
      }
   }
}