using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Weavers.Methods
{
   public class ConstructorAroundWeaver
   {
      public void CreateWeaver(MethodDefinition methodDefinition_P, Type interceptor_P, MethodDefinition wrappedMethod_P)
      {
         var callBaseInstructions = ExtractCallToBaseInstructions(wrappedMethod_P, methodDefinition_P.DeclaringType);
         ClearCallToBase(wrappedMethod_P, methodDefinition_P.DeclaringType);

      }

      private List<Instruction> ExtractCallToBaseInstructions(MethodDefinition wrappedMethod_P, TypeDefinition declaringType_P)
      {
         List<Instruction> instructions_L = new List<Instruction>();
         for (int i = 0; i < wrappedMethod_P.Body.Instructions.Count; i++)
         {
            instructions_L.Add(wrappedMethod_P.Body.Instructions[i]);
            if (IsCallToBase(wrappedMethod_P, declaringType_P, i))
               break;
         }
         return instructions_L;
      }

      private void ClearCallToBase(MethodDefinition wrappedMethod_P, TypeDefinition declaringTypeP_P)
      {
         while (!IsCallToBase(wrappedMethod_P, declaringTypeP_P, 0))
         {
            wrappedMethod_P.Body.Instructions.RemoveAt(0);
         }
         wrappedMethod_P.Body.Instructions.RemoveAt(0);
      }

      private bool IsCallToBase(MethodDefinition wrappedMethod_P, TypeDefinition declaringTypePP_P, int iP_P)
      {
         var instruction_L = wrappedMethod_P.Body.Instructions[0];
         var isCall = instruction_L.OpCode == OpCodes.Call;
         if (!isCall)
            return false;
         var isBase = IsBase(wrappedMethod_P, instruction_L, declaringTypePP_P);
         if (!isBase)
            return false;
         var isCtor = ((MethodReference) instruction_L.Operand).Name == ".ctor";
         return isCtor;
      }

      private static bool IsBase(MethodDefinition wrappedMethod_P, Instruction instruction_L, TypeDefinition declaringTypePPP_P)
      {
         var typeDefinition_L = declaringTypePPP_P;
         //if (typeDefinition_L == null)
         //   typeDefinition_L = declaringTypePPP_P.Module.Import(typeof (object));
         return ((MethodReference) instruction_L.Operand).DeclaringType == typeDefinition_L.BaseType;
      }
   }
}