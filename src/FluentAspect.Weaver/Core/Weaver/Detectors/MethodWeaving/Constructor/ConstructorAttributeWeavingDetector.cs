using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Constructor
{
    public class ConstructorAttributeWeavingDetector
   {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeConstructor.Method != null || 
                aspect.AfterConstructor.Method != null || 
                aspect.OnExceptionConstructor.Method != null || 
                aspect.OnFinallyConstructor.Method != null;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, MethodWeavingModel methodWeavingModel)
        {
           if (!AspectApplier.CanApply(method, aspect, definition => definition.MethodSelector))
                return;
            methodWeavingModel.AddConstructorWeavingModel(method, aspect, aspect.BeforeConstructor, aspect.AfterConstructor, aspect.OnExceptionConstructor,
                                               aspect.OnFinallyConstructor);
        }
    }
}