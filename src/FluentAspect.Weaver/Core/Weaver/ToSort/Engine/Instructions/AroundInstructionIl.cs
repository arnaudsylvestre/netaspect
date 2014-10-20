using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine.Instructions
{
   public class AroundInstructionIl
   {
      public readonly List<Mono.Cecil.Cil.Instruction> AfterInstruction = new List<Mono.Cecil.Cil.Instruction>();
      public readonly List<Mono.Cecil.Cil.Instruction> BeforeInstruction = new List<Mono.Cecil.Cil.Instruction>();
   }
}
