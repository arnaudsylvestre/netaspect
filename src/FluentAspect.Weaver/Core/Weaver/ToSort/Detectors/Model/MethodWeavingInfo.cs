using System.Reflection;
using Mono.Cecil;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Model
{
   public class MethodWeavingInfo
   {
      public MethodDefinition Method { get; set; }
      public MethodInfo Interceptor { get; set; }
   }
}
