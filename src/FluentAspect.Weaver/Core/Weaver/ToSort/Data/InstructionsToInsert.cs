using System.Collections.Generic;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Data
{
   public class InstructionsToInsert
   {

      public List<Mono.Cecil.Cil.Instruction> aspectInitialisation = new List<Mono.Cecil.Cil.Instruction>();
      public List<Mono.Cecil.Cil.Instruction> calledInstructions = new List<Mono.Cecil.Cil.Instruction>();
      public List<Mono.Cecil.Cil.Instruction> calledParametersInstructions = new List<Mono.Cecil.Cil.Instruction>();
      public List<Mono.Cecil.Cil.Instruction> calledParametersObjectInstructions = new List<Mono.Cecil.Cil.Instruction>();
      public List<Mono.Cecil.Cil.Instruction> recallcalledInstructions = new List<Mono.Cecil.Cil.Instruction>();
      public List<Mono.Cecil.Cil.Instruction> recallcalledParametersInstructions = new List<Mono.Cecil.Cil.Instruction>();
      public List<Mono.Cecil.Cil.Instruction> resultInstructions = new List<Mono.Cecil.Cil.Instruction>();

      public List<Mono.Cecil.Cil.Instruction> BeforeInstructions = new List<Mono.Cecil.Cil.Instruction>();
   }
}