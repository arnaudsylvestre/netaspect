using System;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public class FilteredMethodWeavingDetector : WeavingDetector.IMethodWeavingDetector
    {
        private Func<NetAspectDefinition, bool> canHandle;
        private WeavingDetector.IMethodWeavingDetector methodWeavingDetector;

        public FilteredMethodWeavingDetector(WeavingDetector.IMethodWeavingDetector methodWeavingDetector, Func<NetAspectDefinition, bool> canHandle)
        {
            this.methodWeavingDetector = methodWeavingDetector;
            this.canHandle = canHandle;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, AroundMethodWeavingModel methodWeavingModel)
        {
            if (!canHandle(aspect))
                return;
            methodWeavingDetector.DetectWeavingModel(method, aspect, methodWeavingModel);
        }
    }
}