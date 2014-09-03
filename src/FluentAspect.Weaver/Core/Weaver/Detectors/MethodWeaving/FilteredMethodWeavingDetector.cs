using System;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
   public class FilteredMethodWeavingDetector : IMethodWeavingDetector
   {
      private readonly Func<NetAspectDefinition, bool> _canHandle;
      private readonly IMethodWeavingDetector _methodWeavingDetector;

      public FilteredMethodWeavingDetector(IMethodWeavingDetector methodWeavingDetector, Func<NetAspectDefinition, bool> canHandle)
      {
         _methodWeavingDetector = methodWeavingDetector;
         _canHandle = canHandle;
      }

      public AroundMethodWeavingModel DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect)
      {
         return !_canHandle(aspect) ? null : _methodWeavingDetector.DetectWeavingModel(method, aspect);
      }
   }
}
