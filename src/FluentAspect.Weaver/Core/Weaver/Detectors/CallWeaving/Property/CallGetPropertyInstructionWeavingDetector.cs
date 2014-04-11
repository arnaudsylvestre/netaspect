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
                if (IsPropertyCall(instruction, aspect, method))
                {
                    weavingModel.AddGetFieldCallWeavingModel(method, instruction, aspect, aspect.BeforeGetField,
                                                             aspect.AfterGetField);

                }
            }
        }
        
        private static bool IsPropertyCall(Instruction instruction, NetAspectDefinition aspect, MethodDefinition method)
        {
           
            if (instruction.IsACallInstruction())
            {
                var calledMethod = (instruction.Operand as MethodReference).Resolve();
               var properties_L = calledMethod.DeclaringType.Properties;
               foreach (var property_L in properties_L)
               {
                  if (property_L.GetMethod == calledMethod)
                  {
                     return AspectApplier.CanApply(property_L, aspect);
                  }
               }
            }
            return false;
        }
    }
}