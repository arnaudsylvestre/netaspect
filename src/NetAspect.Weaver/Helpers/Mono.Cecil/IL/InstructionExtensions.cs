﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;

namespace NetAspect.Weaver.Helpers.Mono.Cecil.IL
{
   public static class InstructionExtensions
   {
      public static bool IsAnUpdateField(this Instruction instruction)
      {
         return instruction.OpCode == OpCodes.Stsfld ||
                instruction.OpCode == OpCodes.Stfld;
      }

      public static bool IsAGetField(this Instruction instruction)
      {
         return instruction.OpCode == OpCodes.Ldsfld ||
                instruction.OpCode == OpCodes.Ldfld;
      }

      public static bool IsAnUpdatePropertyCall(this Instruction instruction)
      {
         if (!instruction.IsACallInstruction())
            return false;
         MethodDefinition methodDefinition_L = ((MethodReference) instruction.Operand).Resolve();
         PropertyDefinition property = methodDefinition_L.GetPropertyForSetter();
         return (property != null);
      }

      public static bool IsAGetPropertyCall(this Instruction instruction)
      {
         if (!instruction.IsACallInstruction())
            return false;
         MethodDefinition methodDefinition_L = ((MethodReference) instruction.Operand).Resolve();
         PropertyDefinition property = methodDefinition_L.GetPropertyForGetter();
         return (property != null);
      }

      public static MethodDefinition GetCalledMethod(this Instruction instruction)
      {
         return ((MethodReference) instruction.Operand).Resolve();
      }

      public static SequencePoint GetLastSequencePoint(this Instruction instruction)
      {
         if (instruction == null)
            return null;
         if (instruction.SequencePoint != null)
            return instruction.SequencePoint;
         return instruction.Previous.GetLastSequencePoint();
      }


      public static MethodDefinition GetOperandAsMethod(this Instruction instruction)
      {
          return (instruction.Operand as MethodReference).Resolve();
      }
      public static FieldDefinition GetOperandAsField(this Instruction instruction)
      {
          return (instruction.Operand as FieldReference).Resolve();
      }
   }
}
