using System.Linq;
using Mono.Cecil;
using Mono.Collections.Generic;
using NetAspect.Weaver.Core.Model;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Fillers
{
    public class PropertyGetAttributeWeavingDetector : IWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforePropertyGet.Method != null ||
                   aspect.AfterPropertyGet.Method != null ||
                   aspect.OnExceptionPropertyGet.Method != null ||
                   aspect.OnFinallyPropertyGet.Method != null ||
                   aspect.BeforePropertySet.Method != null ||
                   aspect.AfterPropertySet.Method != null ||
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