using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Core
{
   public class NetAspectWeavingMethod
   {
      public readonly List<Instruction> BeforeConstructorBaseCall = new List<Instruction>();
      public readonly List<Instruction> BeforeInstructions = new List<Instruction>();
      public readonly List<Instruction> AfterInstructions = new List<Instruction>();
      public readonly Dictionary<Instruction, InstructionIl> Instructions = new Dictionary<Instruction, InstructionIl>();
      public List<Instruction> OnExceptionInstructions = new List<Instruction>();
      public List<Instruction> OnFinallyInstructions = new List<Instruction>();

      public class InstructionIl
      {
         public List<Instruction> After = new List<Instruction>();
         public List<Instruction> Before = new List<Instruction>();
      }
   }
}
