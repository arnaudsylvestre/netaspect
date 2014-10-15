using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public interface IMethodWeavingDetector
   {
      MethodWeavingAspectInstance DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect);
   }
}
