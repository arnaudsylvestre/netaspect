using System.Linq;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.V2
{
    public class MethodAttributeWeavingModelFiller : IWeavingModelFiller
    {
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


    public class PropertyGetAttributeWeavingModelFiller : IWeavingModelFiller
    {
        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            Collection<PropertyDefinition> propertyDefinitions = method.DeclaringType.Properties;
            foreach (PropertyDefinition propertyDefinition in propertyDefinitions)
            {
                if (propertyDefinition.GetMethod == method)
                {
                    TypeReference aspectType = method.Module.Import(aspect.Type);
                    bool isCompliant_L =
                        propertyDefinition.CustomAttributes.Any(
                            customAttribute_L => customAttribute_L.AttributeType.FullName == aspectType.FullName);
                    if (!isCompliant_L)
                        return;
                    weavingModel.AddPropertyGetWeavingModel(method, aspect, aspect.BeforePropertyGet,
                                                            aspect.AfterPropertyGet, aspect.OnExceptionPropertyGet,
                                                            aspect.OnFinallyPropertyGet);
                }
                if (propertyDefinition.SetMethod == method)
                {
                    TypeReference aspectType = method.Module.Import(aspect.Type);
                    bool isCompliant_L =
                        propertyDefinition.CustomAttributes.Any(
                            customAttribute_L => customAttribute_L.AttributeType.FullName == aspectType.FullName);
                    if (!isCompliant_L)
                        return;
                    weavingModel.AddPropertySetWeavingModel(method, aspect, aspect.BeforePropertySet,
                                                            aspect.AfterPropertySet, aspect.OnExceptionPropertySet,
                                                            aspect.OnFinallyPropertySet);
                }
            }
        }
    }
}