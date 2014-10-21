using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;

namespace NetAspect.Weaver.Core.Weaver.Session
{
   public interface IMethodAspectInstanceDetector
   {
      IEnumerable<AspectInstanceForMethod> GetAspectInstances(MethodDefinition method, NetAspectDefinition aspect);
   }
}
