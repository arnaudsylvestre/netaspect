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
            weavingModel.AddMethodWeavingModel(method, aspect, aspect.Before, aspect.After, aspect.OnException, aspect.OnFinally);
        }

        
    }


    public class PropertyGetAttributeWeavingModelFiller : IWeavingModelFiller
    {
        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            var propertyDefinitions = method.DeclaringType.Properties;
            foreach (var propertyDefinition in propertyDefinitions)
            {
                if (propertyDefinition.GetMethod == method)
                {
                    var aspectType = method.Module.Import(aspect.Type);
                    var isCompliant_L = propertyDefinition.CustomAttributes.Any(customAttribute_L => customAttribute_L.AttributeType.FullName == aspectType.FullName);
                    if (!isCompliant_L)
                        return;
                    weavingModel.AddPropertyGetWeavingModel(method, aspect, aspect.BeforePropertyGet, aspect.AfterPropertyGet, aspect.OnExceptionPropertyGet, aspect.OnFinallyPropertyGet);
                    
                }
            }
        }


    }
}