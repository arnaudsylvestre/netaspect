using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Helpers.Mono.Cecil.IL
{
   public static class InstructionsExtensions
   {
       public static void AppendCallToTargetGetType(this List<Instruction> instructions, ModuleDefinition module, VariableDefinition target)
       {
           instructions.Add(Instruction.Create(OpCodes.Ldloc, target));
           instructions.Add(
              Instruction.Create(
                 OpCodes.Call,
                 module.Import(typeof(object).GetMethod("GetType", new Type[0]))));
       }
       public static void AppendCallToTypeOf(this List<Instruction> instructions, ModuleDefinition module, TypeReference type)
       {
           instructions.Add(Instruction.Create(OpCodes.Ldtoken, type));
           instructions.Add(
              Instruction.Create(
                 OpCodes.Call,
                 module.Import(typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }))));
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

      public static void AppendCreateNewObject(this List<Instruction> instructions, VariableDefinition interceptor, Type interceptorType, ModuleDefinition module, CustomAttribute attribute)
      {
          if (attribute != null)
          {
              for (int i = 0; i < attribute.Constructor.Parameters.Count; i++)
              {
                  instructions.Add(Get(attribute, i)(attribute.ConstructorArguments[i].Value));
              }
              instructions.Add(Instruction.Create(OpCodes.Newobj, attribute.Constructor));
          }
          else
          {
              instructions.Add(Instruction.Create(OpCodes.Newobj, module.Import(interceptorType.GetConstructors()[0])));
          }
         instructions.Add(Instruction.Create(OpCodes.Stloc, interceptor));
      }

       private static Func<object, Instruction> Get(CustomAttribute attribute, int i)
       {
           try
           {
               return adders[attribute.ConstructorArguments[i].Value.GetType()];

           }
           catch (Exception)
           {

               throw;
           }
       }

       public static Dictionary<Type, Func<object, Instruction>> adders = new Dictionary<Type, Func<object, Instruction>>
               {
                   {typeof (string), o => Instruction.Create(OpCodes.Ldstr, o.ToString())},
                   {typeof (int), o => Instruction.Create(OpCodes.Ldc_I4, (int)o)},
                   {typeof (bool), o => Instruction.Create(OpCodes.Ldc_I4, (bool)o ? 1 : 0)},
                   {typeof (byte), o => Instruction.Create(OpCodes.Ldc_I4, (int)(byte)o)},
                   {typeof (char), o => Instruction.Create(OpCodes.Ldc_I4, (int)(char)o)},
                   {typeof (double), o => Instruction.Create(OpCodes.Ldc_R8, (double)o)},
                   {typeof (float), o => Instruction.Create(OpCodes.Ldc_R4, (float)o)},
                   {typeof (long), o => Instruction.Create(OpCodes.Ldc_I8, (long)o)},
                   {typeof (short), o => Instruction.Create(OpCodes.Ldc_I4, (short)o)},
                   {typeof (sbyte), o => Instruction.Create(OpCodes.Ldc_I4, (int)(sbyte)o)},
                   {typeof (uint), o => Instruction.Create(OpCodes.Ldc_I4, (int)(uint)o)},
                   {typeof (ushort), o => Instruction.Create(OpCodes.Ldc_I4, (short)(ushort)o)},
                   {typeof (ulong), o => Instruction.Create(OpCodes.Ldc_I8, (long)(ulong)o)},
               };

       public static void AppendCallStaticMethodAnsSaveResultInto(this List<Instruction> instructions, MethodInfo method, VariableDefinition variable, ModuleDefinition module)
      {
         var methodToCall = module.Import(method);
         instructions.Add(Instruction.Create(OpCodes.Call, methodToCall));
         instructions.Add(Instruction.Create(OpCodes.Stloc, variable));
      }
   }
}
