using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Property
{
    public class PropertyGetAttributeWeavingDetector : IWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeGetProperty.Method != null ||
                   aspect.AfterGetProperty.Method != null ||
                   aspect.OnExceptionPropertyGet.Method != null ||
                   aspect.OnFinallyPropertyGet.Method != null ||
                   aspect.BeforeSetProperty.Method != null ||
                   aspect.AfterSetProperty.Method != null ||
                   aspect.OnExceptionPropertySet.Method != null ||
                   aspect.OnFinallyPropertySet.Method != null;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
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
                    weavingModel.AddPropertyGetWeavingModel(method, aspect, aspect.BeforeGetProperty,
                                                            aspect.AfterGetProperty, aspect.OnExceptionPropertyGet,
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
                    weavingModel.AddPropertySetWeavingModel(method, aspect, aspect.BeforeSetProperty,
                                                            aspect.AfterSetProperty, aspect.OnExceptionPropertySet,
                                                            aspect.OnFinallyPropertySet);
                }
            }
        }
    }
}