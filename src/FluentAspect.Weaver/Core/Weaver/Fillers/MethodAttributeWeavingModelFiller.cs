using System.Linq;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weaver.Engine;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weaver.Fillers
{
    public class MethodAttributeWeavingModelFiller : IWeavingModelFiller
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.Before.Method != null || 
                aspect.After.Method != null || 
                aspect.OnException.Method != null || 
                aspect.OnFinally.Method != null;
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
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