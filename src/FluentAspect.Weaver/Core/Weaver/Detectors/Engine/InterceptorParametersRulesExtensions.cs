using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.ATrier;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Checkers.Ensures;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine
{
   public static class InterceptorParametersRulesExtensions
   {
      public static InterceptorParameterConfiguration WhichCanNotBeReferenced(this InterceptorParameterConfiguration configuration)
      {
         configuration.Checker.Add(Ensure.NotReferenced);
         return configuration;
      }

      public static InterceptorParameterConfiguration WhereParameterTypeIsSameAsMethodResult(this InterceptorParameterConfiguration configuration, MethodWeavingInfo info)
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.ResultOfType(parameter, errorListener, info.Method));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhereParameterTypeIsSameAsMethodResultAndNotReferenced(this InterceptorParameterConfiguration configuration, MethodWeavingInfo info)
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.ResultOfTypeNotReferenced(parameter, errorListener, info.Method));
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheVariable(this InterceptorParameterConfiguration configuration, Func<IlInjectorAvailableVariables, VariableDefinition> variableProvider)
      {
         configuration.Generator.Generators.Add(
            (parameterInfo, instructions, info) =>
               instructions.Add(
                  Instruction.Create(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, variableProvider(info))
                  ));
         return configuration;
      }


      public static InterceptorParameterConfiguration WhichPdbPresent(this InterceptorParameterConfiguration configuration, InstructionWeavingInfo info)
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.SequencePoint(info.Instruction, errorListener, parameter));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhichPdbPresentForMethod(this InterceptorParameterConfiguration configuration, MethodWeavingInfo info)
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.SequencePoint(GetFirstInstruction(info), errorListener, parameter));
         return configuration;
      }

      private static Instruction GetFirstInstruction(MethodWeavingInfo info)
      {
         return info.Method.Body.Instructions.First();
      }

      public static InterceptorParameterConfiguration WhichCanNotBeOut(this InterceptorParameterConfiguration configuration)
      {
         configuration.Checker.Add(new ParameterReferencedChecker(ParameterReferencedChecker.ReferenceModel.Referenced));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhereFieldCanNotBeStatic(this InterceptorParameterConfiguration configuration, IMemberDefinition member)
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.NotStaticButDefaultValue(parameter, errorListener, member));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhereCurrentMethodCanNotBeStatic(this InterceptorParameterConfiguration configuration, MethodWeavingInfo weavingInfo)
      {
         configuration.Checker.Add((parameter, errorListener) => Ensure.NotStatic(parameter, errorListener, weavingInfo.Method));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhichMustBeOfType<T1>(this InterceptorParameterConfiguration configuration)
      {
         return configuration.WhichMustBeOfTypeOf(typeof (T1).FullName);
      }

      public static InterceptorParameterConfiguration WhichMustBeOfTypeOfParameter(this InterceptorParameterConfiguration configuration, ParameterDefinition parameterDefinition)
      {
         configuration.Checker.Add((info, handler) => Ensure.OfType(info, handler, parameterDefinition));
         return configuration;
      }

      public static InterceptorParameterConfiguration OrOfType(this InterceptorParameterConfiguration configuration, TypeReference type)
      {
         return WhichMustBeOfTypeOf(configuration, type);
      }

      public static InterceptorParameterConfiguration WhichMustBeOfTypeOf(this InterceptorParameterConfiguration configuration, TypeReference type)
      {
         WhichMustBeOfTypeOf(configuration, type.FullName);
         return configuration;
      }

      public static InterceptorParameterConfiguration WhichMustBeOfTypeOf(this InterceptorParameterConfiguration configuration, string fullName)
      {
         configuration.Checker.Add(new ParameterTypeChecker(fullName, null));
         return configuration;
      }

      public static InterceptorParameterConfiguration OrOfCurrentMethodDeclaringType(this InterceptorParameterConfiguration configuration, MethodWeavingInfo weavingInfo)
      {
         return OrOfType(configuration, weavingInfo.Method.DeclaringType);
      }


      public static InterceptorParameterConfiguration AndInjectThePdbInfo(this InterceptorParameterConfiguration configuration, Func<SequencePoint, int> pdbInfoProvider, InstructionWeavingInfo weavingInfo)
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

      public static InterceptorParameterConfiguration AndInjectThePdbInfoForMethod(this InterceptorParameterConfiguration configuration, Func<SequencePoint, int> pdbInfoProvider, MethodWeavingInfo weavingInfo)
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

      public static InterceptorParameterConfiguration AndInjectThePdbInfo(this InterceptorParameterConfiguration configuration, Func<SequencePoint, string> pdbInfoProvider, InstructionWeavingInfo weavingInfo_P)
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

      public static InterceptorParameterConfiguration AndInjectThePdbInfoForMethod(this InterceptorParameterConfiguration configuration, Func<SequencePoint, string> pdbInfoProvider, MethodWeavingInfo weavingInfo_P)
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


      public static InterceptorParameterConfiguration AndInjectTheCalledPropertyInfo(this InterceptorParameterConfiguration configuration, InstructionWeavingInfo weavingInfo)
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               InstructionWeavingInfo interceptor = weavingInfo;
               instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
               instructions.AppendCallToGetProperty(interceptor.GetOperandAsMethod().GetProperty().Name, interceptor.Method.Module);
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheCalledInstance(this InterceptorParameterConfiguration configuration)
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               VariableDefinition called = info.Called;
               instructions.Add(
                  called == null
                     ? Instruction.Create(OpCodes.Ldnull)
                     : Instruction.Create(OpCodes.Ldloc, called));
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheCurrentInstance(this InterceptorParameterConfiguration configuration)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => { instructions.Add(Instruction.Create(OpCodes.Ldarg_0)); });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheCurrentMethod(this InterceptorParameterConfiguration configuration)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => { instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodBase)); });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheCurrentProperty(this InterceptorParameterConfiguration configuration)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => { instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentPropertyInfo)); });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheValue(this InterceptorParameterConfiguration configuration, string value)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => { instructions.Add(Instruction.Create(OpCodes.Ldstr, value)); });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheParameterInfo(this InterceptorParameterConfiguration configuration, ParameterDefinition parameterDefinition, MethodDefinition method)
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodBase));
               instructions.Add(Instruction.Create(OpCodes.Callvirt, method.Module.Import(typeof (MethodBase).GetMethod("GetParameters"))));
               instructions.Add(Instruction.Create(OpCodes.Ldc_I4, method.Parameters.IndexOf(parameterDefinition)));
               instructions.Add(Instruction.Create(OpCodes.Ldelem_Ref));
            });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheCalledFieldInfo(this InterceptorParameterConfiguration configuration, InstructionWeavingInfo weavingInfo_P)
      {
         configuration.Generator.Generators.Add(
            (parameter, instructions, info) =>
            {
               FieldDefinition fieldReference = weavingInfo_P.GetOperandAsField();
               ModuleDefinition module = weavingInfo_P.Method.Module;
               instructions.AppendCallToTargetGetType(module, info.Called);
               instructions.AppendCallToGetField(fieldReference.Name, module);
            });
         return configuration;
      }

      public static void AndInjectTheCalledParameter(this InterceptorParameterConfiguration configuration,
         ParameterDefinition parameter)
      {
         AndInjectTheCalledParameter(configuration, parameter, p => "called" + p.Name);
      }

      public static void AndInjectTheCalledValue(this InterceptorParameterConfiguration configuration,
         ParameterDefinition parameter)
      {
         AndInjectTheCalledParameter(configuration, parameter, p => p.Name);
      }

      public static void AndInjectTheFieldValue(this InterceptorParameterConfiguration configuration,
         FieldDefinition field)
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
                        info.FieldValue));
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
                        info.FieldValue));
               }
               if (field.FieldType != moduleDefinition.TypeSystem.Object &&
                   parameterInfo.ParameterType == typeof (Object))
               {
                  TypeReference reference = info.FieldValue.VariableType;

                  instructions.Add(Instruction.Create(OpCodes.Box, reference));
               }
            });
      }

      private static void AndInjectTheCalledParameter(this InterceptorParameterConfiguration configuration, ParameterDefinition parameter, Func<ParameterDefinition, string> nameComputer)
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
                        info.CalledParameters[parameterName]));
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
                        info.CalledParameters[parameterName]));
               }
               if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                   parameterInfo.ParameterType == typeof (Object))
               {
                  TypeReference reference = info.CalledParameters[parameterName].VariableType;
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


      public static InterceptorParameterConfiguration AndInjectTheParameter(this InterceptorParameterConfiguration configuration, ParameterDefinition parameter)
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
