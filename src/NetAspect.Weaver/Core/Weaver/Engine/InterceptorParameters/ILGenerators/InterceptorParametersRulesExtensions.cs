using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine
{

    public static class InterceptorParametersRulesExtensions
   {

       // TODO : A mettre dans plusieurs fonctions
      

      public static InterceptorParameterPossibility<T> AndInjectTheVariable<T>(this InterceptorParameterPossibility<T> possibility, Func<T, VariableDefinition> variableProvider) where T : VariablesForMethod
      {
         possibility.Generators.Add(
            (parameterInfo, instructions, info) =>
               instructions.Add(
                  Mono.Cecil.Cil.Instruction.Create(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, variableProvider(info))
                  ));
         return possibility;
      }



      


      public static InterceptorParameterPossibility<T> AndInjectThePdbInfo<T>(this InterceptorParameterPossibility<T> possibility, Func<SequencePoint, int> pdbInfoProvider, InstructionWeavingInfo weavingInfo) where T : VariablesForMethod
      {
         possibility.Generators.Add(
            (parameter, instructions, info) =>
            {
               SequencePoint instructionPP = weavingInfo.Instruction.GetLastSequencePoint();
               instructions.Add(
                  Mono.Cecil.Cil.Instruction.Create(
                     OpCodes.Ldc_I4,
                     instructionPP == null
                        ? 0
                        : pdbInfoProvider(instructionPP)));
            });
         return possibility;
      }

      public static InterceptorParameterPossibility<T> AndInjectThePdbInfoForMethod<T>(this InterceptorParameterPossibility<T> possibility, Func<SequencePoint, int> pdbInfoProvider, CommonWeavingInfo weavingInfo) where T : VariablesForMethod
      {
         possibility.Generators.Add(
            (parameter, instructions, info) =>
            {
                SequencePoint instruction = weavingInfo.Method.Body.Instructions.First().GetLastSequencePoint();
               instructions.Add(
                  Mono.Cecil.Cil.Instruction.Create(
                     OpCodes.Ldc_I4,
                     instruction == null
                        ? 0
                        : pdbInfoProvider(instruction)));
            });
         return possibility;
      }

      public static InterceptorParameterPossibility<T> AndInjectThePdbInfo<T>(this InterceptorParameterPossibility<T> possibility, Func<SequencePoint, string> pdbInfoProvider, InstructionWeavingInfo weavingInfo_P) where T : VariablesForMethod
      {
         possibility.Generators.Add(
            (parameter, instructions, info) =>
            {
               SequencePoint instructionPP = weavingInfo_P.Instruction.GetLastSequencePoint();
               instructions.Add(
                  Mono.Cecil.Cil.Instruction.Create(
                     OpCodes.Ldstr,
                     instructionPP == null
                        ? null
                        : pdbInfoProvider(instructionPP)));
            });
         return possibility;
      }

      public static InterceptorParameterPossibility<T> AndInjectThePdbInfoForMethod<T>(this InterceptorParameterPossibility<T> possibility, Func<SequencePoint, string> pdbInfoProvider, CommonWeavingInfo weavingInfo) where T : VariablesForMethod
      {
         possibility.Generators.Add(
            (parameter, instructions, info) =>
            {
                SequencePoint instructionPP = weavingInfo.Method.Body.Instructions.First().GetLastSequencePoint();
               instructions.Add(
                  Mono.Cecil.Cil.Instruction.Create(
                     OpCodes.Ldstr,
                     instructionPP == null
                        ? null
                        : pdbInfoProvider(instructionPP)));
            });
         return possibility;
      }


      public static InterceptorParameterPossibility<VariablesForInstruction> AndInjectTheCalledPropertyInfo(this InterceptorParameterPossibility<VariablesForInstruction> possibility, InstructionWeavingInfo weavingInfo)
      {
         possibility.Generators.Add(
            (parameter, instructions, info) =>
            {
               InstructionWeavingInfo interceptor = weavingInfo;
               var propertyDefinition = interceptor.GetOperandAsMethod().GetProperty();
                var definition = info.Called.Definition;
                if (definition != null)
                    instructions.AppendCallToTargetGetType(interceptor.Method.Module, definition);
                else
                    instructions.AppendCallToTypeOf(interceptor.Method.Module, propertyDefinition.DeclaringType);
                instructions.AppendCallToGetProperty(propertyDefinition.Name, interceptor.Method.Module);
            });
         return possibility;
      }

      public static InterceptorParameterPossibility<VariablesForInstruction> AndInjectTheCalledInstance(this InterceptorParameterPossibility<VariablesForInstruction> possibility) 
      {
         possibility.Generators.Add(
            (parameter, instructions, info) =>
            {
               VariableDefinition called = info.Called.Definition;
               instructions.Add(
                  called == null
                     ? Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldnull)
                     : Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, called));
            });
         return possibility;
      }

      public static InterceptorParameterPossibility<T> AndInjectTheCurrentInstance<T>(this InterceptorParameterPossibility<T> possibility) where T : VariablesForMethod
      {
         possibility.Generators.Add((parameter, instructions, info) => instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldarg_0)));
         return possibility;
      }

      public static InterceptorParameterPossibility<T> AndInjectTheCurrentMethod<T>(this InterceptorParameterPossibility<T> possibility) where T : VariablesForMethod
      {
         possibility.Generators.Add((parameter, instructions, info) => instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, info.CallerMethod.Definition)));
         return possibility;
      }

      public static InterceptorParameterPossibility<T> AndInjectTheCurrentProperty<T>(this InterceptorParameterPossibility<T> possibility) where T : VariablesForMethod
      {
         possibility.Generators.Add((parameter, instructions, info) => instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, info.CallerProperty.Definition)));
         return possibility;
      }

      public static InterceptorParameterPossibility<T> AndInjectTheValue<T>(this InterceptorParameterPossibility<T> possibility, string value) where T : VariablesForMethod
      {
         possibility.Generators.Add((parameter, instructions, info) => instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldstr, value)));
         return possibility;
      }

      public static InterceptorParameterPossibility<T> AndInjectTheParameterInfo<T>(this InterceptorParameterPossibility<T> possibility, ParameterDefinition parameterDefinition, MethodDefinition method) where T : VariablesForMethod
      {
         possibility.Generators.Add(
            (parameter, instructions, info) =>
            {
               instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, info.CallerMethod.Definition));
               instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Callvirt, method.Module.Import(typeof (MethodBase).GetMethod("GetParameters"))));
               instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldc_I4, method.Parameters.IndexOf(parameterDefinition)));
               instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldelem_Ref));
            });
         return possibility;
      }

      public static InterceptorParameterPossibility<VariablesForInstruction> AndInjectTheCalledFieldInfo(this InterceptorParameterPossibility<VariablesForInstruction> possibility, InstructionWeavingInfo weavingInfo_P)
      {
          possibility.Generators.Add(
             (parameter, instructions, info) =>
             {
                 FieldDefinition fieldReference = weavingInfo_P.GetOperandAsField();
                 ModuleDefinition module = weavingInfo_P.Method.Module;
                 var definition = info.Called.Definition;
                 if (definition != null)
                     instructions.AppendCallToTargetGetType(module, definition);
                 else
                 {
                     instructions.AppendCallToTypeOf(module, fieldReference.DeclaringType);
                 }
                 instructions.AppendCallToGetField(fieldReference.Name, module);
             });
          return possibility;
      }

      public static InterceptorParameterPossibility<VariablesForInstruction> AndInjectTheCalledConstructorInfo(this InterceptorParameterPossibility<VariablesForInstruction> possibility, InstructionWeavingInfo weavingInfo_P)
      {
          possibility.Generators.Add(
             (parameter, instructions, info) =>
             {
                 var methodReference = weavingInfo_P.GetOperandAsMethod();
                 ModuleDefinition module = weavingInfo_P.Method.Module;
                 var definition = info.Called.Definition;
                 if (definition != null)
                     instructions.AppendCallToTargetGetType(module, definition);
                 else
                 {
                     instructions.AppendCallToTypeOf(module, methodReference.DeclaringType);
                 }
                 instructions.AppendCallToGetConstructor(methodReference, module, info.AddLocalVariable);
             });
          return possibility;
      }

      public static void AndInjectTheCalledParameter(this InterceptorParameterPossibility<VariablesForInstruction> possibility,
         ParameterDefinition parameter)
      {
         AndInjectTheCalledParameter(possibility, parameter, p => "called" + p.Name);
      }

      public static void AndInjectTheCalledValue(this InterceptorParameterPossibility<VariablesForInstruction> possibility,
         ParameterDefinition parameter)
      {
         AndInjectTheCalledParameter(possibility, parameter, p => p.Name);
      }

      public static void AndInjectTheFieldValue<T>(this InterceptorParameterPossibility<T> possibility,
         FieldDefinition field) where T : VariablesForInstruction
      {
         possibility.Generators.Add(
            (parameterInfo, instructions, info) =>
            {
               ModuleDefinition moduleDefinition = field.Module;
               if (!parameterInfo.ParameterType.IsByRef && field.FieldType.IsByReference)
               {
                  instructions.Add(
                     Mono.Cecil.Cil.Instruction.Create(
                        OpCodes.Ldloc,
                        info.FieldValue.Definition));
                  instructions.Add(
                     Mono.Cecil.Cil.Instruction.Create(
                        OpCodes.Ldobj,
                        moduleDefinition.Import(parameterInfo.ParameterType)));
               }
               else
               {
                  instructions.Add(
                     Mono.Cecil.Cil.Instruction.Create(
                        OpCodes.Ldloc,
                        info.FieldValue.Definition));
               }
               if (field.FieldType != moduleDefinition.TypeSystem.Object &&
                   parameterInfo.ParameterType == typeof (Object))
               {
                  TypeReference reference = info.FieldValue.Definition.VariableType;

                  instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Box, reference));
               }
            });
      }

      private static void AndInjectTheCalledParameter(this InterceptorParameterPossibility<VariablesForInstruction> possibility, ParameterDefinition parameter, Func<ParameterDefinition, string> nameComputer)
      {
         possibility.Generators.Add(
            (parameterInfo, instructions, info) =>
            {
               ModuleDefinition moduleDefinition = ((MethodDefinition) parameter.Method).Module;
               string parameterName = nameComputer(parameter);
               if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
               {
                  instructions.Add(
                     Mono.Cecil.Cil.Instruction.Create(
                        OpCodes.Ldloc,
                        info.CalledParameters.GetDefinition(parameterName)));
                  instructions.Add(
                     Mono.Cecil.Cil.Instruction.Create(
                        OpCodes.Ldobj,
                        moduleDefinition.Import(parameterInfo.ParameterType)));
               }
               else
               {
                  instructions.Add(
                     Mono.Cecil.Cil.Instruction.Create(
                        OpCodes.Ldloc,
                        info.CalledParameters.GetDefinition(parameterName)));
               }
               if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                   parameterInfo.ParameterType == typeof (Object))
               {
                  TypeReference reference = info.CalledParameters.GetDefinition(parameterName).VariableType;
                  if (reference.IsByReference)
                  {
                     reference =
                        ((MethodDefinition) parameter.Method).GenericParameters.First(
                           t => t.Name == reference.Name.TrimEnd('&'));
                     instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldobj, reference));
                  }
                  instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Box, reference));
               }
            });
      }


      public static InterceptorParameterPossibility<T> AndInjectTheParameter<T>(this InterceptorParameterPossibility<T> possibility, ParameterDefinition parameter) where T : VariablesForMethod
      {
         possibility.Generators.Add(
            (parameterInfo, instructions, info) =>
            {
               ModuleDefinition moduleDefinition = ((MethodDefinition) parameter.Method).Module;
               if (parameterInfo.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
               {
                  instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldarga, parameter));
               }
               else if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
               {
                  instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldarg, parameter));
                  instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(parameterInfo.ParameterType)));
               }
               else
               {
                  instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldarg, parameter));
               }
               if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                   parameterInfo.ParameterType == typeof (Object))
               {
                  TypeReference reference = parameter.ParameterType;
                  if (reference.IsByReference)
                  {
                     reference =
                        ((MethodDefinition) parameter.Method).GenericParameters.First(
                           t => t.Name == reference.Name.TrimEnd('&'));
                     instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldobj, reference));
                  }

                  if (parameter.ParameterType.IsValueType || parameter.ParameterType is GenericParameter)
                     instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Box, reference));
               }
            });
         return possibility;
      }
   }
}
