using System.Linq;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public class MethodAttributeWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.Before.Method != null || 
                aspect.After.Method != null || 
                aspect.OnException.Method != null || 
                aspect.OnFinally.Method != null;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, MethodWeavingModel methodWeavingModel)
        {
            TypeReference aspectType = method.Module.Import(aspect.Type);
            bool isCompliant_L =
                method.CustomAttributes.Any(
                    customAttribute_L => customAttribute_L.AttributeType.FullName == aspectType.FullName);
            if (!isCompliant_L)
                return;
            methodWeavingModel.AddMethodWeavingModel(method, aspect, aspect.Before, aspect.After, aspect.OnException,
                                               aspect.OnFinally);
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, AroundMethodWeavingModel methodWeavingModel)
        {
            if (!AspectApplier.CanApply(method, aspect, definition => definition.MethodSelector))
                return;
        }
    }
}