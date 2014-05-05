using Mono.Cecil;
using Mono.Collections.Generic;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Property
{
    public class PropertyGetAttributeWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforePropertyGetMethod.Method != null ||
                aspect.AfterPropertyGetMethod.Method != null ||
                aspect.OnExceptionPropertyGetMethod.Method != null ||
                aspect.OnFinallyPropertyGetMethod.Method != null ||
                aspect.BeforePropertySetMethod.Method != null ||
                aspect.AfterPropertySetMethod.Method != null ||
                aspect.OnExceptionPropertySetMethod.Method != null ||
                aspect.OnFinallyPropertySetMethod.Method != null;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, MethodWeavingModel methodWeavingModel)
        {
            Collection<PropertyDefinition> propertyDefinitions = method.DeclaringType.Properties;
            foreach (PropertyDefinition propertyDefinition in propertyDefinitions)
            {
                if (propertyDefinition.GetMethod == method)
                {
                    bool isCompliant_L = AspectApplier.CanApply(propertyDefinition, aspect, definition => definition.PropertySelector);
                    if (!isCompliant_L)
                        return;
                    methodWeavingModel.AddPropertyGetMethodWeavingModel(method, aspect, aspect.BeforePropertyGetMethod,
                                                            aspect.AfterPropertyGetMethod, aspect.OnExceptionPropertyGetMethod,
                                                            aspect.OnFinallyPropertyGetMethod);
                }
                if (propertyDefinition.SetMethod == method)
                {
                    bool isCompliant_L = AspectApplier.CanApply(propertyDefinition, aspect, definition => definition.PropertySelector);
                    if (!isCompliant_L)
                        return;
                    methodWeavingModel.AddPropertySetMethodWeavingModel(method, aspect, aspect.BeforePropertySetMethod,
                                                            aspect.AfterPropertySetMethod, aspect.OnExceptionPropertySetMethod,
                                                            aspect.OnFinallyPropertySetMethod);
                }
            }
        }
    }
}