using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Model
{
   public class InstructionWeavingInfo : MethodWeavingInfo
    {
        public Instruction Instruction { get; set; }
    }
}