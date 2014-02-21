using System.Linq;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public class MethodAttributeWeavingModelFiller : IWeavingModelFiller
    {
        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            var aspectType = method.Module.Import(aspect.Type);
            var isCompliant_L = method.CustomAttributes.Any(customAttribute_L => customAttribute_L.AttributeType.FullName == aspectType.FullName);
            if (!isCompliant_L)
                return;
            if (aspect.Before.Method != null)
            {
                weavingModel.Method.Befores.Add(MethodWeavingMethodInjectorFactory.CreateForBefore(method, aspect.Before.Method, aspect.Type));
            }
            if (aspect.After.Method != null)
            {
                weavingModel.Method.Befores.Add(MethodWeavingMethodInjectorFactory.CreateForAfter(method, aspect.After.Method, aspect.Type));
            }
            if (aspect.OnException.Method != null)
            {
                weavingModel.Method.Befores.Add(MethodWeavingMethodInjectorFactory.CreateForOnException(method, aspect.OnException.Method, aspect.Type));
            }
            if (aspect.OnFinally.Method != null)
            {
                weavingModel.Method.Befores.Add(MethodWeavingMethodInjectorFactory.CreateForBefore(method, aspect.OnFinally.Method, aspect.Type));
            }
        }
    }
}