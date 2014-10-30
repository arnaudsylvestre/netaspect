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
                    typeof(Type).GetMethod(
                       "GetField",
                       new[] { typeof(string), typeof(BindingFlags) }))));
       }

       public static void AppendCallToGetConstructor(this List<Instruction> instructions,
          MethodReference methodReference,
          ModuleDefinition module, Action<VariableDefinition> addVariable, VariableDefinition typeInstance)
       {
           var typesFromParameters_L = CreateTypesFromParameters(instructions, methodReference);
           instructions.Add(Instruction.Create(OpCodes.Ldloc, typeInstance));
           instructions.Add(Instruction.Create(OpCodes.Ldc_I4, ComputeBindingFlags(methodReference)));
           instructions.Add(Instruction.Create(OpCodes.Ldnull));
           addVariable(typesFromParameters_L);
           instructions.Add(Instruction.Create(OpCodes.Ldloc, typesFromParameters_L));
           instructions.Add(Instruction.Create(OpCodes.Ldnull));
           instructions.Add(
              Instruction.Create(
                 OpCodes.Callvirt,
                 module.Import(
                    typeof(Type).GetMethod(
                       "GetConstructor",
                       new[] { typeof(BindingFlags), typeof(Binder), typeof(Type[]), typeof(ParameterModifier[]) }))));
       }

       private static int ComputeBindingFlags(MethodReference methodReference)
       {
           BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic;
           flags |= methodReference.Resolve().IsStatic ? BindingFlags.Static : BindingFlags.Instance;
           return (int) flags;
       }

       public static void AppendCallToGetMethod(this List<Instruction> instructions,
          MethodReference methodReference,
          ModuleDefinition module, Action<VariableDefinition> addVariable, VariableDefinition typeInstance)
       {
           var typesFromParameters_L = CreateTypesFromParameters(instructions, methodReference);
           instructions.Add(Instruction.Create(OpCodes.Ldloc, typeInstance));
           instructions.Add(Instruction.Create(OpCodes.Ldstr, methodReference.Name));
           instructions.Add(Instruction.Create(OpCodes.Ldc_I4, ComputeBindingFlags(methodReference)));
           instructions.Add(Instruction.Create(OpCodes.Ldnull));
           addVariable(typesFromParameters_L);
           instructions.Add(Instruction.Create(OpCodes.Ldloc, typesFromParameters_L));
           instructions.Add(Instruction.Create(OpCodes.Ldnull));
           instructions.Add(
               Instruction.Create(
                   OpCodes.Callvirt,
                   module.Import(
                       typeof(Type).GetMethod(
                           "GetMethod",
                           new[] { typeof(string), typeof(BindingFlags), typeof(Binder), typeof(Type[]), typeof(ParameterModifier[]) }))));
       }


       private static VariableDefinition CreateTypesFromParameters(List<Instruction> instructions, MethodReference methodReference)
       {
           var tabVariable = new VariableDefinition(methodReference.Module.Import(typeof(Type[])));
           instructions.Add(Instruction.Create(OpCodes.Ldc_I4, methodReference.Parameters.Count));
           instructions.Add(Instruction.Create(OpCodes.Newarr, methodReference.Module.Import(typeof(Type))));
           instructions.Add(Instruction.Create(OpCodes.Stloc_S, tabVariable));
           int i = 0;
           foreach (var parameter in methodReference.Parameters)
           {
               instructions.Add(Instruction.Create(OpCodes.Ldloc_S, tabVariable));
               instructions.Add(Instruction.Create(OpCodes.Ldc_I4, i));
               AppendCallToTypeOf(instructions, methodReference.Module, parameter.ParameterType);
               instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
               i++;

           }
           return tabVariable;
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
           return adders[attribute.ConstructorArguments[i].Value.GetType()];
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
   }
}
