using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Engine.Instructions
{
   public class AroundInstructionIl
   {
      public readonly List<Instruction> BeforeInstruction = new List<Instruction>();
      public readonly List<Instruction> AfterInstruction = new List<Instruction>();
   }
}