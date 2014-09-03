using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Data
{
   public class InstructionsToInsert
   {

      public List<Instruction> calledInstructions = new List<Instruction>();
      public List<Instruction> calledParametersInstructions = new List<Instruction>();
      public List<Instruction> calledParametersObjectInstructions = new List<Instruction>();
      public List<Instruction> recallcalledInstructions = new List<Instruction>();
      public List<Instruction> recallcalledParametersInstructions = new List<Instruction>();
      public List<Instruction> resultInstructions = new List<Instruction>();

      public List<Instruction> BeforeInstructions = new List<Instruction>();
   }
}