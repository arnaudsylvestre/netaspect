using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Checkers.Ensures;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine
{
   public static class InterceptorParametersRulesExtensions
   {
       public static InterceptorParameterConfiguration<T> WhichCanNotBeReferenced<T>(this InterceptorParameterConfiguration<T> configuration) where T : VariablesForMethod
       {
         configuration.Checker.Add(Ensure.NotReferenced);
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> WhereParameterTypeIsSameAsMethodResult<T>(this InterceptorParameterConfiguration<T> configuration, MethodWeavingInfo info) where T : VariablesForMethod
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.ResultOfType(parameter, errorListener, info.Method));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> WhereParameterTypeIsSameAsMethodResultAndNotReferenced<T>(this InterceptorParameterConfiguration<T> configuration, MethodWeavingInfo info) where T : VariablesForMethod
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.ResultOfTypeNotReferenced(parameter, errorListener, info.Method));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectTheVariable<T>(this InterceptorParameterConfiguration<T> configuration, Func<T, VariableDefinition> variableProvider) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add(
            (parameterInfo, instructions, info) =>
               instructions.Add(
                  Instruction.Create(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, variableProvider(info))
                  ));
         return configuration;
      }


      public static InterceptorParameterConfiguration<T> WhichPdbPresent<T>(this InterceptorParameterConfiguration<T> configuration, InstructionWeavingInfo info) where T : VariablesForMethod
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.SequencePoint(info.Instruction, errorListener, parameter));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> WhichPdbPresentForMethod<T>(this InterceptorParameterConfiguration<T> configuration, MethodWeavingInfo info) where T : VariablesForMethod
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.SequencePoint(GetFirstInstruction(info), errorListener, parameter));
         return configuration;
      }

      private static Instruction GetFirstInstruction(MethodWeavingInfo info)
      {
         return info.Method.Body.Instructions.First();
      }

      public static InterceptorParameterConfiguration<T> WhichCanNotBeOut<T>(this InterceptorParameterConfiguration<T> configuration) where T : VariablesForMethod
      {
         configuration.Checker.Add(new ParameterReferencedChecker(ParameterReferencedChecker.ReferenceModel.Referenced));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> WhereFieldCanNotBeStatic<T>(this InterceptorParameterConfiguration<T> configuration, IMemberDefinition member) where T : VariablesForMethod
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.NotStaticButDefaultValue(parameter, errorListener, member));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> WhereCurrentMethodCanNotBeStatic<T>(this InterceptorParameterConfiguration<T> configuration, MethodWeavingInfo weavingInfo) where T : VariablesForMethod
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.NotStatic(parameter, errorListener, weavingInfo.Method));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> WhichMustBeOfType<T, T1>(this InterceptorParameterConfiguration<T> configuration) where T : VariablesForMethod
      {
         return configuration.WhichMustBeOfTypeOf(typeof (T1).FullName);
      }

      public static InterceptorParameterConfiguration<T> WhichMustBeOfTypeOfParameter<T>(this InterceptorParameterConfiguration<T> configuration, ParameterDefinition parameterDefinition) where T : VariablesForMethod
      {
         configuration.Checker.Add((info, handler) => Ensure.ParameterOfType(info, handler, parameterDefinition));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> OrOfType<T>(this InterceptorParameterConfiguration<T> configuration, TypeReference type) where T : VariablesForMethod
      {
         return WhichMustBeOfTypeOf(configuration, type);
      }

      public static InterceptorParameterConfiguration<T> WhichMustBeOfTypeOf<T>(this InterceptorParameterConfiguration<T> configuration, TypeReference type) where T : VariablesForMethod
      {
         WhichMustBeOfTypeOf(configuration, type.FullName);
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> WhichMustBeOfTypeOf<T>(this InterceptorParameterConfiguration<T> configuration, string fullName) where T : VariablesForMethod
      {
         configuration.Checker.Add(new ParameterTypeChecker(fullName, null));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> OrOfCurrentMethodDeclaringType<T>(this InterceptorParameterConfiguration<T> configuration, MethodWeavingInfo weavingInfo) where T : VariablesForMethod
      {
         return OrOfType(configuration, weavingInfo.Method.DeclaringType);
      }


      public static InterceptorParameterConfiguration<T> AndInjectThePdbInfo<T>(this InterceptorParameterConfiguration<T> configuration, Func<SequencePoint, int> pdbInfoProvider, InstructionWeavingInfo weavingInfo) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               SequencePoint instructionPP = weavingInfo.Instruction.GetLastSequencePoint();
               instructions.Add(
                  Instruction.Create(
                     OpCodes.Ldc_I4,
                     instructionPP == null
                        ? 0
                        : pdbInfoProvider(instructionPP)));
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectThePdbInfoForMethod<T>(this InterceptorParameterConfiguration<T> configuration, Func<SequencePoint, int> pdbInfoProvider, MethodWeavingInfo weavingInfo) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               SequencePoint instruction = GetFirstInstruction(weavingInfo).GetLastSequencePoint();
               instructions.Add(
                  Instruction.Create(
                     OpCodes.Ldc_I4,
                     instruction == null
                        ? 0
                        : pdbInfoProvider(instruction)));
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectThePdbInfo<T>(this InterceptorParameterConfiguration<T> configuration, Func<SequencePoint, string> pdbInfoProvider, InstructionWeavingInfo weavingInfo_P) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               SequencePoint instructionPP = weavingInfo_P.Instruction.GetLastSequencePoint();
               instructions.Add(
                  Instruction.Create(
                     OpCodes.Ldstr,
                     instructionPP == null
                        ? null
                        : pdbInfoProvider(instructionPP)));
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectThePdbInfoForMethod<T>(this InterceptorParameterConfiguration<T> configuration, Func<SequencePoint, string> pdbInfoProvider, MethodWeavingInfo weavingInfo_P) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               SequencePoint instructionPP = GetFirstInstruction(weavingInfo_P).GetLastSequencePoint();
               instructions.Add(
                  Instruction.Create(
                     OpCodes.Ldstr,
                     instructionPP == null
                        ? null
                        : pdbInfoProvider(instructionPP)));
            });
         return configuration;
      }


      public static InterceptorParameterConfiguration<VariablesForInstruction> AndInjectTheCalledPropertyInfo(this InterceptorParameterConfiguration<VariablesForInstruction> configuration, InstructionWeavingInfo weavingInfo)
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               InstructionWeavingInfo interceptor = weavingInfo;
               instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called.Definition);
               instructions.AppendCallToGetProperty(interceptor.GetOperandAsMethod().GetProperty().Name, interceptor.Method.Module);
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration<VariablesForInstruction> AndInjectTheCalledInstance(this InterceptorParameterConfiguration<VariablesForInstruction> configuration) 
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               VariableDefinition called = info.Called.Definition;
               instructions.Add(
                  called == null
                     ? Instruction.Create(OpCodes.Ldnull)
                     : Instruction.Create(OpCodes.Ldloc, called));
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectTheCurrentInstance<T>(this InterceptorParameterConfiguration<T> configuration) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectTheCurrentMethod<T>(this InterceptorParameterConfiguration<T> configuration) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CallerMethod.Definition)));
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectTheCurrentProperty<T>(this InterceptorParameterConfiguration<T> configuration) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => { instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CallerProperty.Definition)); });
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectTheValue<T>(this InterceptorParameterConfiguration<T> configuration, string value) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => { instructions.Add(Instruction.Create(OpCodes.Ldstr, value)); });
         return configuration;
      }

      public static InterceptorParameterConfiguration<T> AndInjectTheParameterInfo<T>(this InterceptorParameterConfiguration<T> configuration, ParameterDefinition parameterDefinition, MethodDefinition method) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CallerMethod.Definition));
               instructions.Add(Instruction.Create(OpCodes.Callvirt, method.Module.Import(typeof (MethodBase).GetMethod("GetParameters"))));
               instructions.Add(Instruction.Create(OpCodes.Ldc_I4, method.Parameters.IndexOf(parameterDefinition)));
               instructions.Add(Instruction.Create(OpCodes.Ldelem_Ref));
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration<VariablesForInstruction> AndInjectTheCalledFieldInfo(this InterceptorParameterConfiguration<VariablesForInstruction> configuration, InstructionWeavingInfo weavingInfo_P)
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               FieldDefinition fieldReference = weavingInfo_P.GetOperandAsField();
               ModuleDefinition module = weavingInfo_P.Method.Module;
               instructions.AppendCallToTargetGetType(module, info.Called.Definition);
               instructions.AppendCallToGetField(fieldReference.Name, module);
            });
         return configuration;
      }

      public static void AndInjectTheCalledParameter(this InterceptorParameterConfiguration<VariablesForInstruction> configuration,
         ParameterDefinition parameter)
      {
         AndInjectTheCalledParameter(configuration, parameter, p => "called" + p.Name);
      }

      public static void AndInjectTheCalledValue(this InterceptorParameterConfiguration<VariablesForInstruction> configuration,
         ParameterDefinition parameter)
      {
         AndInjectTheCalledParameter(configuration, parameter, p => p.Name);
      }

      public static void AndInjectTheFieldValue<T>(this InterceptorParameterConfiguration<T> configuration,
         FieldDefinition field) where T : VariablesForInstruction
      {
         configuration.Generator.Generators.Add(
            (parameterInfo, instructions, info) =>
            {
               ModuleDefinition moduleDefinition = field.Module;
               if (!parameterInfo.ParameterType.IsByRef && field.FieldType.IsByReference)
               {
                  instructions.Add(
                     Instruction.Create(
                        OpCodes.Ldloc,
                        info.FieldValue.Definition));
                  instructions.Add(
                     Instruction.Create(
                        OpCodes.Ldobj,
                        moduleDefinition.Import(parameterInfo.ParameterType)));
               }
               else
               {
                  instructions.Add(
                     Instruction.Create(
                        OpCodes.Ldloc,
                        info.FieldValue.Definition));
               }
               if (field.FieldType != moduleDefinition.TypeSystem.Object &&
                   parameterInfo.ParameterType == typeof (Object))
               {
                  TypeReference reference = info.FieldValue.Definition.VariableType;

                  instructions.Add(Instruction.Create(OpCodes.Box, reference));
               }
            });
      }

      private static void AndInjectTheCalledParameter(this InterceptorParameterConfiguration<VariablesForInstruction> configuration, ParameterDefinition parameter, Func<ParameterDefinition, string> nameComputer)
      {
         configuration.Generator.Generators.Add(
            (parameterInfo, instructions, info) =>
            {
               ModuleDefinition moduleDefinition = ((MethodDefinition) parameter.Method).Module;
               string parameterName = nameComputer(parameter);
               if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
               {
                  instructions.Add(
                     Instruction.Create(
                        OpCodes.Ldloc,
                        info.CalledParameters.GetDefinition(parameterName)));
                  instructions.Add(
                     Instruction.Create(
                        OpCodes.Ldobj,
                        moduleDefinition.Import(parameterInfo.ParameterType)));
               }
               else
               {
                  instructions.Add(
                     Instruction.Create(
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
                     instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));
                  }
                  instructions.Add(Instruction.Create(OpCodes.Box, reference));
               }
            });
      }


      public static InterceptorParameterConfiguration<T> AndInjectTheParameter<T>(this InterceptorParameterConfiguration<T> configuration, ParameterDefinition parameter) where T : VariablesForMethod
      {
         configuration.Generator.Generators.Add(
            (parameterInfo, instructions, info) =>
            {
               ModuleDefinition moduleDefinition = ((MethodDefinition) parameter.Method).Module;
               if (parameterInfo.ParameterType.IsByRef && !parameter.ParameterType.IsByReference)
               {
                  instructions.Add(Instruction.Create(OpCodes.Ldarga, parameter));
               }
               else if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
               {
                  instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
                  instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(parameterInfo.ParameterType)));
               }
               else
               {
                  instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
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
                     instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));
                  }

                  if (parameter.ParameterType.IsValueType || parameter.ParameterType is GenericParameter)
                     instructions.Add(Instruction.Create(OpCodes.Box, reference));
               }
            });
         return configuration;
      }
   }
}
