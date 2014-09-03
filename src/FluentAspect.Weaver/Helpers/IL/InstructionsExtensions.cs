using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Helpers.IL
{
   public static class InstructionsExtensions
   {
      public static void AppendCallToTargetGetType(this List<Instruction> instructions, ModuleDefinition module, VariableDefinition target)
      {
         instructions.Add(Instruction.Create(OpCodes.Ldloc, target));
         instructions.Add(
            Instruction.Create(
               OpCodes.Call,
               module.Import(typeof (object).GetMethod("GetType", new Type[0]))));
      }

      public static void AppendCallToGetField(this List<Instruction> instructions,
         string fieldName,
         ModuleDefinition module)
      {
         instructions.Add(Instruction.Create(OpCodes.Ldstr, fieldName));
         instructions.Add(Instruction.Create(OpCodes.Ldc_I4, 60));
         instructions.Add(
            Instruction.Create(
               OpCodes.Callvirt,
               module.Import(
                  typeof (Type).GetMethod(
                     "GetField",
                     new[] {typeof (string), typeof (BindingFlags)}))));
      }

      public static void AppendCallToGetMethod(this List<Instruction> instructions,
         string methodName,
         ModuleDefinition module)
      {
         instructions.Add(Instruction.Create(OpCodes.Ldstr, methodName));
         instructions.Add(Instruction.Create(OpCodes.Ldc_I4, 60));
         instructions.Add(
            Instruction.Create(
               OpCodes.Callvirt,
               module.Import(
                  typeof (Type).GetMethod(
                     "GetMethod",
                     new[] {typeof (string), typeof (BindingFlags)}))));
      }

      public static void AppendCallToGetProperty(this List<Instruction> instructions,
         string propertyName,
         ModuleDefinition module)
      {
         instructions.Add(Instruction.Create(OpCodes.Ldstr, propertyName));
         instructions.Add(Instruction.Create(OpCodes.Ldc_I4, 60));
         instructions.Add(
            Instruction.Create(
               OpCodes.Callvirt,
               module.Import(
                  typeof (Type).GetMethod(
                     "GetProperty",
                     new[] {typeof (string), typeof (BindingFlags)}))));
      }

      public static void AppendSaveResultTo(this List<Instruction> instructions, VariableDefinition variable)
      {
         instructions.Add(Instruction.Create(OpCodes.Stloc, variable));
      }

      public static void AppendCreateNewObject(this List<Instruction> instructions,
         VariableDefinition interceptor,
         Type interceptorType,
         ModuleDefinition module)
      {
         instructions.Add(Instruction.Create(OpCodes.Newobj, module.Import(interceptorType.GetConstructors()[0])));
         instructions.Add(Instruction.Create(OpCodes.Stloc, interceptor));
      }
   }
}
