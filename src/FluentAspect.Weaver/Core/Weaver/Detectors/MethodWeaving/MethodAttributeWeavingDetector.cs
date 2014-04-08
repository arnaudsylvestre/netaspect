using System.Linq;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public class MethodAttributeWeavingDetector : IWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.Before.Method != null || 
                aspect.After.Method != null || 
                aspect.OnException.Method != null || 
                aspect.OnFinally.Method != null;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            TypeReference aspectType = method.Module.Import(aspect.Type);
            bool isCompliant_L =
                method.CustomAttributes.Any(
                    customAttribute_L => customAttribute_L.AttributeType.FullName == aspectType.FullName);
            if (!isCompliant_L)
                return;
            weavingModel.AddMethodWeavingModel(method, aspect, aspect.Before, aspect.After, aspect.OnException,
                                               aspect.OnFinally);
        }
    }
}