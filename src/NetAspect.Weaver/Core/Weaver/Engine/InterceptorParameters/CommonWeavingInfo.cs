using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model
{
   public class CommonWeavingInfo
   {
      public MethodDefinition Method { get; set; }
      public IEnumerable<MethodInfo> Interceptor { get; set; }
   }
}
