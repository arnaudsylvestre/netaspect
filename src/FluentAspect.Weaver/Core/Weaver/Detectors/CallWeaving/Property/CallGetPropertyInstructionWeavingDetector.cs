using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{
    public class CallGetPropertyInstructionWeavingDetector : IWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeGetProperty.Method != null ||
                   aspect.AfterGetProperty.Method != null;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            if (method.Body == null)
                return;
            foreach (var instruction in method.Body.Instructions)
            {
                if (IsPropertyCall(instruction, aspect))
                {
                    var calledMethod = (instruction.Operand as MethodReference).Resolve();
                    weavingModel.AddGetPropertyCallWeavingModel(method, instruction, aspect, aspect.BeforeGetProperty,
                                                             aspect.AfterGetProperty, GetPropertyForGetter(calledMethod));

                }
            }
        }
        
        private static bool IsPropertyCall(Instruction instruction, NetAspectDefinition aspect)
        {
           
            if (instruction.IsACallInstruction())
            {
                var calledMethod = (instruction.Operand as MethodReference).Resolve();
                var property_L = GetPropertyForGetter(calledMethod);
                if (property_L != null)
                     return AspectApplier.CanApply(property_L, aspect);
                  
               
            }
            return false;
        }

        private static PropertyDefinition GetPropertyForGetter(MethodDefinition getMethod)
        {
            var properties_L = getMethod.DeclaringType.Properties;
            return properties_L.FirstOrDefault(property_L => property_L.GetMethod == getMethod);
        }
    }
}