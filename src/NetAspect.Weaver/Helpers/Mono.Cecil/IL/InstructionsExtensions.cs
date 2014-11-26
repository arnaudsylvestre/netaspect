using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Helpers.Mono.Cecil.IL
{
    using MethodAttributes = global::Mono.Cecil.MethodAttributes;
    using ParameterAttributes = global::Mono.Cecil.ParameterAttributes;

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
          ModuleDefinition module, VariableDefinition typeInstance, MethodDefinition methodDefinition)
       {
           var methodGetMethod = new MethodDefinition(Guid.NewGuid().ToString(), MethodAttributes.Private | MethodAttributes.Static, module.Import(typeof(MethodInfo)));
           methodGetMethod.Parameters.Add(new ParameterDefinition("type", ParameterAttributes.None, module.Import(typeof(Type))));
           methodDefinition.DeclaringType.Methods.Add(methodGetMethod);
           CreateMethodToGetMethodInfo(methodGetMethod, methodReference, module);
           instructions.Add(Instruction.Create(OpCodes.Ldloc, typeInstance));
           instructions.Add(Instruction.Create(OpCodes.Call, methodGetMethod));
       }

        private static void CreateMethodToGetMethodInfo(
            MethodDefinition methodDefinition, MethodReference methodReference, ModuleDefinition module)
        {
            var methods = (new VariableDefinition(module.Import(typeof(MethodInfo[]))));
            var method = (new VariableDefinition(module.Import(typeof(MethodInfo))));
            var finalMethod = (new VariableDefinition(module.Import(typeof(MethodInfo))));
            var i = (new VariableDefinition(module.Import(typeof(int))));
            var condition = (new VariableDefinition(module.Import(typeof(bool))));
            var parameters = (new VariableDefinition(module.Import(typeof(ParameterInfo[]))));
            methodDefinition.Body.Variables.Add(methods);
            methodDefinition.Body.Variables.Add(method);
            methodDefinition.Body.Variables.Add(finalMethod);
            methodDefinition.Body.Variables.Add(i);
            methodDefinition.Body.Variables.Add(condition);
            methodDefinition.Body.Variables.Add(parameters);
            methodDefinition.Body.InitLocals = true;

            methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, ComputeBindingFlags(methodReference)));
            methodDefinition.Body.Instructions.Add(
                Instruction.Create(
                    OpCodes.Callvirt, module.Import(typeof(Type).GetMethod("GetMethods", new[] { typeof(BindingFlags) }))));
            methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc, methods));

            var startCondition = Instruction.Create(OpCodes.Nop);
            var startLoop = Instruction.Create(OpCodes.Nop);
            var startIncrementation = Instruction.Create(OpCodes.Nop);
            var endLoop = Instruction.Create(OpCodes.Nop);

            methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4_0));
            methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Stloc_S, i));
            methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Br, startCondition));
            methodDefinition.Body.Instructions.Add(startLoop);

            methodDefinition.Body.Instructions.AddRange(GetMethodAtIndex(methods, i, method));

            methodDefinition.Body.Instructions.AddRange(CheckMethodName(module, method, methodReference, condition, startIncrementation));
            methodDefinition.Body.Instructions.AddRange(CheckGenericArguments(module, method, condition, startIncrementation, methodReference));
            methodDefinition.Body.Instructions.AddRange(
                CheckParametersLength(module, method, methodReference, condition, startIncrementation, parameters));
            for (int j = 0; j < methodReference.Parameters.Count; j++)
            {
                methodDefinition.Body.Instructions.AddRange(
                    CheckParameterType(module, methodReference, condition, startIncrementation, parameters, j));
            }

            methodDefinition.Body.Instructions.AddRange(SaveMethod(finalMethod, method, endLoop));

            methodDefinition.Body.Instructions.Add(startIncrementation);
            methodDefinition.Body.Instructions.AddRange(CreateLoopIncrementation(i));
            methodDefinition.Body.Instructions.Add(startCondition);
            methodDefinition.Body.Instructions.AddRange(CreateLoopCondition(i, methods, startLoop, condition));
            methodDefinition.Body.Instructions.Add(endLoop);
            methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, finalMethod));
            methodDefinition.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
        }

        private static IEnumerable<Instruction> SaveMethod(VariableDefinition finalMethod, VariableDefinition method, Instruction endLoop)
       {
           var instructions = new List<Instruction>
                {
                    Instruction.Create(OpCodes.Ldloc, method),
                    Instruction.Create(OpCodes.Stloc, finalMethod),
                    Instruction.Create(OpCodes.Br, endLoop),
                };
           /*	IL_00ab: ldloc.2
		IL_00ac: stloc.0
		IL_00ad: br.s IL_00c6*/
           return instructions;
       }

       private static IEnumerable<Instruction> CheckParameterType(ModuleDefinition module, MethodReference methodReference, VariableDefinition condition, Instruction startIncrementation, VariableDefinition parameters, int parameterIndex)
       {
           var parameterDefinition = methodReference.Parameters[parameterIndex];
           var getParameterType = module.Import(typeof(ParameterInfo).GetMethod("get_ParameterType", new Type[] { }));
           var getName = module.Import(typeof(MemberInfo).GetMethod("get_Name", new Type[] { }));
           var getFullName = module.Import(typeof(Type).GetMethod("get_FullName", new Type[] { }));
           var stringInequalityMethod = module.Import(typeof(string).GetMethod("op_Inequality", new[] { typeof(string), typeof(string) }));
           var instructions = new List<Instruction>
                {
                    Instruction.Create(OpCodes.Ldloc, parameters),
                    Instruction.Create(OpCodes.Ldc_I4, parameterIndex),
                    Instruction.Create(OpCodes.Ldelem_Ref),
                    Instruction.Create(OpCodes.Callvirt, getParameterType),
                    Instruction.Create(OpCodes.Callvirt, parameterDefinition.ParameterType.IsGenericParameter ? getName : getFullName),
                    Instruction.Create(OpCodes.Ldstr, parameterDefinition.ParameterType.IsGenericParameter ? parameterDefinition.ParameterType.Name : parameterDefinition.ParameterType.FullName.Replace("/", "+")),
                    Instruction.Create(OpCodes.Call, stringInequalityMethod),
                    Instruction.Create(OpCodes.Ldc_I4_0),
                    Instruction.Create(OpCodes.Ceq),
                    Instruction.Create(OpCodes.Stloc_S, condition),
                    Instruction.Create(OpCodes.Ldloc_S, condition),
                    Instruction.Create(OpCodes.Brfalse, startIncrementation)

                };
           return instructions;
       }

       private static IEnumerable<Instruction> CheckParametersLength(ModuleDefinition module, VariableDefinition method, MethodReference methodReference, VariableDefinition condition, Instruction startIncrementation, VariableDefinition parameters)
       {
           var getParametersMethod = module.Import(typeof(MethodBase).GetMethod("GetParameters", new Type[] { }));
           var instructions = new List<Instruction>
                {
                    Instruction.Create(OpCodes.Ldloc, method),
                    Instruction.Create(OpCodes.Callvirt, getParametersMethod),
                    Instruction.Create(OpCodes.Stloc, parameters),
                    Instruction.Create(OpCodes.Ldloc, parameters),
                    Instruction.Create(OpCodes.Ldlen),
                    Instruction.Create(OpCodes.Conv_I4),
                    Instruction.Create(OpCodes.Ldc_I4, methodReference.Parameters.Count),
                    Instruction.Create(OpCodes.Ceq),
                    Instruction.Create(OpCodes.Stloc_S, condition),
                    Instruction.Create(OpCodes.Ldloc_S, condition),
                    Instruction.Create(OpCodes.Brfalse, startIncrementation)

                };

          /* IL_0052: ldloc.2
		IL_0053: callvirt instance class [mscorlib]System.Reflection.ParameterInfo[] [mscorlib]System.Reflection.MethodBase::GetParameters()
		IL_0058: stloc.3
		IL_0059: ldloc.3
		IL_005a: ldlen
		IL_005b: conv.i4
		IL_005c: ldc.i4.2
		IL_005d: ceq
		IL_005f: stloc.s CS$4$0003
		IL_0061: ldloc.s CS$4$0003
		IL_0063: brtrue.s IL_0067*/
           return instructions;
       }

       private static IEnumerable<Instruction> CheckGenericArguments(ModuleDefinition module, VariableDefinition method, VariableDefinition condition, Instruction startIncrementation, MethodReference methodReference)
       {
           var getGenericArguments = module.Import(typeof(MethodBase).GetMethod("GetGenericArguments", new Type[] { }));
          var instructions = new List<Instruction>
                {
                    Instruction.Create(OpCodes.Ldloc, method),
                    Instruction.Create(OpCodes.Callvirt, getGenericArguments),
                    Instruction.Create(OpCodes.Ldlen),
                    Instruction.Create(OpCodes.Conv_I4),
                    Instruction.Create(OpCodes.Ldc_I4, methodReference.GenericParameters.Count),
                    Instruction.Create(OpCodes.Ceq),
                    Instruction.Create(OpCodes.Stloc_S, condition),
                    Instruction.Create(OpCodes.Ldloc_S, condition),
                    Instruction.Create(OpCodes.Brfalse, startIncrementation),
                };
           /*
                    IL_003f: ldloc.2
		IL_0040: callvirt instance class [mscorlib]System.Type[] [mscorlib]System.Reflection.MethodBase::GetGenericArguments()
		IL_0045: ldlen
		IL_0046: conv.i4
		IL_0047: ldc.i4.1
		IL_0048: ceq
		IL_004a: stloc.s CS$4$0003
		IL_004c: ldloc.s CS$4$0003
		IL_004e: brtrue.s IL_0052
            */
           return instructions;
       }

       private static List<Instruction> CheckMethodName(ModuleDefinition module, VariableDefinition method, MethodReference methodReference, VariableDefinition condition, Instruction startIncrementation)
       {
           var getNameMethod = module.Import(typeof(MemberInfo).GetMethod("get_Name", new Type[] { }));
           var stringInequalityMethod = module.Import(typeof(string).GetMethod("op_Inequality", new[] { typeof(string), typeof(string) }));
           var instructions = new List<Instruction>
                {
                    Instruction.Create(OpCodes.Ldloc, method),
                    Instruction.Create(OpCodes.Callvirt, getNameMethod),
                    Instruction.Create(OpCodes.Ldstr, methodReference.Name),
                    Instruction.Create(OpCodes.Call, stringInequalityMethod),
                    Instruction.Create(OpCodes.Ldc_I4_0),
                    Instruction.Create(OpCodes.Ceq),
                    Instruction.Create(OpCodes.Stloc_S, condition),
                    Instruction.Create(OpCodes.Ldloc_S, condition),
                    Instruction.Create(OpCodes.Brfalse, startIncrementation)
                };
           return instructions;
           /*IL_0024: ldloc.2
		IL_0025: callvirt instance string [mscorlib]System.Reflection.MemberInfo::get_Name()
		IL_002a: ldstr "Weaved"
		IL_002f: call bool [mscorlib]System.String::op_Inequality(string, string)
		IL_0034: ldc.i4.0
		IL_0035: ceq
		IL_0037: stloc.s CS$4$0003
		IL_0039: ldloc.s CS$4$0003
		IL_003b: brtrue.s IL_003f*/
       }

       private static List<Instruction> CreateLoopIncrementation(VariableDefinition i)
       {
           var instructions = new List<Instruction>
                {
                    Instruction.Create(OpCodes.Ldloc_S, i),
                    Instruction.Create(OpCodes.Ldc_I4_1),
                    Instruction.Create(OpCodes.Add),
                    Instruction.Create(OpCodes.Stloc_S, i),
                };
           return instructions;
       }

       private static List<Instruction> GetMethodAtIndex(VariableDefinition methods, VariableDefinition i, VariableDefinition method)
       {

           var instructions = new List<Instruction>
                    {
                        Instruction.Create(OpCodes.Ldloc_S, methods),
                        Instruction.Create(OpCodes.Ldloc_S, i),
                        Instruction.Create(OpCodes.Ldelem_Ref),
                        Instruction.Create(OpCodes.Stloc, method)
                    };
           return instructions;
           //   IL_001d: ldloc.s CS$6$0001
           //IL_001f: ldloc.s CS$7$0002
           //IL_0021: ldelem.ref
           //IL_0022: stloc.2
       }

       private static List<Instruction> CreateLoopCondition(VariableDefinition i, VariableDefinition methods, Instruction startLoop, VariableDefinition condition)
       {
           var instructions = new List<Instruction>
                {
                    Instruction.Create(OpCodes.Ldloc_S, i),
                    Instruction.Create(OpCodes.Ldloc_S, methods),
                    Instruction.Create(OpCodes.Ldlen),
                    Instruction.Create(OpCodes.Conv_I4),
                    Instruction.Create(OpCodes.Clt),
                    Instruction.Create(OpCodes.Stloc_S, condition),
                    Instruction.Create(OpCodes.Ldloc_S, condition),
                    Instruction.Create(OpCodes.Brtrue, startLoop)
                };
           return instructions;
           //    IL_00c3: ldloc.s CS$7$0002
           //IL_00c5: ldloc.s CS$6$0001
           //IL_00c7: ldlen
           //IL_00c8: conv.i4
           //IL_00c9: clt
           //IL_00cb: stloc.s CS$4$0003
           //IL_00cd: ldloc.s CS$4$0003
           //IL_00cf: brtrue IL_001d
       }

       /**
   
   // loop start (head: IL_00c3)
       IL_001d: ldloc.s CS$6$0001
       IL_001f: ldloc.s CS$7$0002
       IL_0021: ldelem.ref
       IL_0022: stloc.2
       IL_0023: nop
       IL_0024: ldloc.2
       IL_0025: callvirt instance string [mscorlib]System.Reflection.MemberInfo::get_Name()
       IL_002a: ldstr "Weaved"
       IL_002f: call bool [mscorlib]System.String::op_Inequality(string, string)
       IL_0034: ldc.i4.0
       IL_0035: ceq
       IL_0037: stloc.s CS$4$0003
       IL_0039: ldloc.s CS$4$0003
       IL_003b: brtrue.s IL_003f

       IL_003d: br.s IL_00bd

       IL_003f: ldloc.2
       IL_0040: callvirt instance class [mscorlib]System.Type[] [mscorlib]System.Reflection.MethodBase::GetGenericArguments()
       IL_0045: ldlen
       IL_0046: conv.i4
       IL_0047: ldc.i4.1
       IL_0048: ceq
       IL_004a: stloc.s CS$4$0003
       IL_004c: ldloc.s CS$4$0003
       IL_004e: brtrue.s IL_0052

       IL_0050: br.s IL_00bd

       IL_0052: ldloc.2
       IL_0053: callvirt instance class [mscorlib]System.Reflection.ParameterInfo[] [mscorlib]System.Reflection.MethodBase::GetParameters()
       IL_0058: stloc.3
       IL_0059: ldloc.3
       IL_005a: ldlen
       IL_005b: conv.i4
       IL_005c: ldc.i4.2
       IL_005d: ceq
       IL_005f: stloc.s CS$4$0003
       IL_0061: ldloc.s CS$4$0003
       IL_0063: brtrue.s IL_0067

       IL_0065: br.s IL_00bd

       IL_0067: ldloc.3
       IL_0068: ldc.i4.0
       IL_0069: ldelem.ref
       IL_006a: callvirt instance class [mscorlib]System.Type [mscorlib]System.Reflection.ParameterInfo::get_ParameterType()
       IL_006f: stloc.s parameterType
       IL_0071: ldloc.s parameterType
       IL_0073: callvirt instance string [mscorlib]System.Reflection.MemberInfo::get_Name()
       IL_0078: ldstr "T"
       IL_007d: call bool [mscorlib]System.String::op_Inequality(string, string)
       IL_0082: ldc.i4.0
       IL_0083: ceq
       IL_0085: stloc.s CS$4$0003
       IL_0087: ldloc.s CS$4$0003
       IL_0089: brtrue.s IL_008d

       IL_008b: br.s IL_00bd

       IL_008d: ldloc.3
       IL_008e: ldc.i4.1
       IL_008f: ldelem.ref
       IL_0090: callvirt instance class [mscorlib]System.Type [mscorlib]System.Reflection.ParameterInfo::get_ParameterType()
       IL_0095: callvirt instance string [mscorlib]System.Type::get_FullName()
       IL_009a: ldtoken [mscorlib]System.String
       IL_009f: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
       IL_00a4: callvirt instance string [mscorlib]System.Type::get_FullName()
       IL_00a9: call bool [mscorlib]System.String::op_Inequality(string, string)
       IL_00ae: ldc.i4.0
       IL_00af: ceq
       IL_00b1: stloc.s CS$4$0003
       IL_00b3: ldloc.s CS$4$0003
       IL_00b5: brtrue.s IL_00b9

       IL_00b7: br.s IL_00bd

       IL_00b9: ldloc.2
       IL_00ba: stloc.0
       IL_00bb: br.s IL_00d4

       IL_00bd: ldloc.s CS$7$0002
       IL_00bf: ldc.i4.1
       IL_00c0: add
       IL_00c1: stloc.s CS$7$0002

       IL_00c3: ldloc.s CS$7$0002
       IL_00c5: ldloc.s CS$6$0001
       IL_00c7: ldlen
       IL_00c8: conv.i4
       IL_00c9: clt
       IL_00cb: stloc.s CS$4$0003
       IL_00cd: ldloc.s CS$4$0003
       IL_00cf: brtrue IL_001d
   // end loop

   IL_00d4: ldloc.0
   IL_00d5: ldstr "Elle est nulle !!!"
   IL_00da: call void [nunit.framework]NUnit.Framework.Assert::NotNull(object, string)
   IL_00df: nop
   IL_00e0: ldarg.1
   IL_00e1: stloc.s CS$1$0000
   IL_00e3: br.s IL_00e5

   IL_00e5: ldloc.s CS$1$0000
   IL_00e7: ret
} // end of method ClassToWeaveCheckMethod::Weaved

    * 
    * 
    */

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
