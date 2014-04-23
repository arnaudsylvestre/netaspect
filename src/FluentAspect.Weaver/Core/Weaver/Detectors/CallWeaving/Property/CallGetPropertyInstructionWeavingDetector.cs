using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Property
{
    public class CallGetPropertyInstructionWeavingDetector : IWeavingDetector
    {
        public bool CanHandle(NetAspectDefinition aspect)
        {
            return aspect.BeforeGetProperty.Method != null ||
                   aspect.AfterGetProperty.Method != null ||
                   aspect.BeforeSetProperty.Method != null ||
                   aspect.AfterSetProperty.Method != null;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, MethodWeavingModel methodWeavingModel)
        {
            if (method.Body == null)
                return;
            foreach (var instruction in method.Body.Instructions)
            {
               if (IsGetPropertyCall(instruction, aspect))
               {
                  var calledMethod = (instruction.Operand as MethodReference).Resolve();
                  methodWeavingModel.AddGetPropertyCallWeavingModel(method, instruction, aspect, aspect.BeforeGetProperty,
                                                           aspect.AfterGetProperty, calledMethod.GetPropertyForGetter());

               }
               if (IsSetPropertyCall(instruction, aspect))
               {
                  var calledMethod = (instruction.Operand as MethodReference).Resolve();
                  methodWeavingModel.AddSetPropertyCallWeavingModel(method, instruction, aspect, aspect.BeforeSetProperty,
                                                           aspect.AfterSetProperty, calledMethod.GetPropertyForSetter());

               }
            }
        }
        
        private static bool IsGetPropertyCall(Instruction instruction, NetAspectDefinition aspect)
        {
           
            if (instruction.IsACallInstruction())
            {
                var calledMethod = (instruction.Operand as MethodReference).Resolve();
                var property_L = calledMethod.GetPropertyForGetter();
                if (property_L != null)
                     return AspectApplier.CanApply(property_L, aspect);
                  
               
            }
            return false;
        }

        private static bool IsSetPropertyCall(Instruction instruction, NetAspectDefinition aspect)
        {

           if (instruction.IsACallInstruction())
           {
              var calledMethod = (instruction.Operand as MethodReference).Resolve();
              var property_L = calledMethod.GetPropertyForSetter();
              if (property_L != null)
                 return AspectApplier.CanApply(property_L, aspect);


           }
           return false;
        }

        
    }
}