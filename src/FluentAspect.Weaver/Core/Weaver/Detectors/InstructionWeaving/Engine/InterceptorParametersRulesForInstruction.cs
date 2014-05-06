using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Helpers;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine
{

   public static class InterceptorParametersRulesExtensions
   {
      public static InterceptorParameterConfiguration WhichCanNotBeReferenced(this InterceptorParameterConfiguration configuration)
      {
         configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotReferenced(parameter, errorListener));
         return configuration;
      }


      public static InterceptorParameterConfiguration AndInjectTheVariable(this InterceptorParameterConfiguration configuration, Func<IlInjectorAvailableVariables, VariableDefinition> variableProvider)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) => 
               instructions.Add(Instruction.Create(OpCodes.Ldloc, variableProvider(info))
         ));
         return configuration;
      }



      public static InterceptorParameterConfiguration WhichPdbPresent(this InterceptorParameterConfiguration configuration, InstructionWeavingInfo info)
      {
         configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.SequencePoint(info.Instruction, errorListener, parameter));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhichCanNotBeOut(this InterceptorParameterConfiguration configuration)
      {
         configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotOut(parameter, errorListener));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhereFieldCanNotBeStatic(this InterceptorParameterConfiguration configuration, InstructionWeavingInfo weavingInfo)
      {
         configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotStaticButDefaultValue(parameter, errorListener, weavingInfo.GetOperandAsField()));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhereCurrentMethodCanNotBeStatic(this InterceptorParameterConfiguration configuration, MethodWeavingInfo weavingInfo)
      {
         configuration.Checker.Checkers.Add((parameter, errorListener) => Ensure.NotStatic(parameter, errorListener, weavingInfo.Method));
         return configuration;
      }

      public static InterceptorParameterConfiguration WhichMustBeOfType<T1>(this InterceptorParameterConfiguration configuration)
      {
         //allowedTypes.Add(typeof(T1).FullName);
         return configuration;
      }

      public static InterceptorParameterConfiguration WhichMustBeOfTypeOfParameter(this InterceptorParameterConfiguration configuration, ParameterDefinition parameterDefinition)
      {
         configuration.Checker.Checkers.Add((info, handler) => Ensure.OfType(info, handler, parameterDefinition));
         return configuration;
      }

      public static InterceptorParameterConfiguration OrOfType(this InterceptorParameterConfiguration configuration, TypeReference type)
      {
         return WhichMustBeOfTypeOf(configuration, type);
      }

      public static InterceptorParameterConfiguration WhichMustBeOfTypeOf(this InterceptorParameterConfiguration configuration, TypeReference type)
      {
         //allowedTypes.Add(type.FullName);
         return configuration;
      }

      public static InterceptorParameterConfiguration OrOfFieldDeclaringType(this InterceptorParameterConfiguration configuration, InstructionWeavingInfo weavingInfo)
      {
         return OrOfType(configuration, weavingInfo.GetOperandAsField().DeclaringType);
      }

      public static InterceptorParameterConfiguration OrOfCurrentMethodDeclaringType(this InterceptorParameterConfiguration configuration, MethodWeavingInfo weavingInfo)
      {
         return OrOfType(configuration, weavingInfo.Method.DeclaringType);
      }


      public static InterceptorParameterConfiguration AndInjectThePdbInfo(this InterceptorParameterConfiguration configuration, Func<SequencePoint, int> pdbInfoProvider, InstructionWeavingInfo weavingInfo)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) =>
         {
            SequencePoint instructionPP = weavingInfo.Instruction.GetLastSequencePoint();
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4,
                                                instructionPP == null
                                                    ? 0
                                                    : pdbInfoProvider(instructionPP)));
         });
         return configuration;
      }
      public static InterceptorParameterConfiguration AndInjectThePdbInfo(this InterceptorParameterConfiguration configuration, Func<SequencePoint, string> pdbInfoProvider, InstructionWeavingInfo weavingInfo_P)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) =>
         {
            SequencePoint instructionPP = weavingInfo_P.Instruction.GetLastSequencePoint();
            instructions.Add(Instruction.Create(OpCodes.Ldstr,
                                                instructionPP == null
                                                    ? null
                                                    : pdbInfoProvider(instructionPP)));
         });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheCalledFieldInfo(this InterceptorParameterConfiguration configuration, InstructionWeavingInfo weavingInfo)
      {

         configuration.Generator.Generators.Add((parameter, instructions, info) =>
         {
            var interceptor = weavingInfo;
            instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
            instructions.AppendCallToGetField(interceptor.GetOperandAsField().Name, interceptor.Method.Module);
         });
         return configuration;
      }
      public static InterceptorParameterConfiguration AndInjectTheCalledPropertyInfo(this InterceptorParameterConfiguration configuration, InstructionWeavingInfo weavingInfo)
      {

         configuration.Generator.Generators.Add((parameter, instructions, info) =>
         {
            var interceptor = weavingInfo;
            instructions.AppendCallToTargetGetType(interceptor.Method.Module, info.Called);
            instructions.AppendCallToGetProperty(interceptor.GetOperandAsMethod().GetProperty().Name, interceptor.Method.Module);
         });
         return configuration;
      }

      public static InterceptorParameterConfiguration AndInjectTheCalledInstance(this InterceptorParameterConfiguration configuration)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) =>
         {
            var called = info.Called;
            instructions.Add(called == null
                                 ? Instruction.Create(OpCodes.Ldnull)
                                 : Instruction.Create(OpCodes.Ldloc, called));
         });
         return configuration;
      }
      public static InterceptorParameterConfiguration AndInjectTheCurrentInstance(this InterceptorParameterConfiguration configuration)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) =>
         {
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
         });
         return configuration;
      }
      public static InterceptorParameterConfiguration AndInjectTheCurrentMethod(this InterceptorParameterConfiguration configuration)
      {
         configuration.Generator.Generators.Add((parameter, instructions, info) =>
         {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodBase));
         });
         return configuration;
      }

      public static void AndInjectTheCalledParameter(this InterceptorParameterConfiguration configuration, ParameterDefinition parameter)
      {
         configuration.Generator.Generators.Add((parameterInfo, instructions, info) =>
         {
            ModuleDefinition moduleDefinition = ((MethodDefinition)parameter.Method).Module;
            if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
            {
               instructions.Add(Instruction.Create(OpCodes.Ldloc,
                                                   info.CalledParameters["called" + parameter.Name]));
               instructions.Add(Instruction.Create(OpCodes.Ldobj,
                                                   moduleDefinition.Import(parameterInfo.ParameterType)));
            }
            else
            {
               instructions.Add(Instruction.Create(OpCodes.Ldloc,
                                                   info.CalledParameters["called" + parameter.Name]));
            }
            if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
                parameterInfo.ParameterType == typeof(Object))
            {
               TypeReference reference = parameter.ParameterType;
               if (reference.IsByReference)
               {
                  reference =
                      ((MethodDefinition)parameter.Method).GenericParameters.First(
                          t => t.Name == reference.Name.TrimEnd('&'));
                  instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));
               }
               instructions.Add(Instruction.Create(OpCodes.Box, reference));
            }
         });
      }


      public static InterceptorParameterConfiguration AndInjectTheParameter(this InterceptorParameterConfiguration configuration, ParameterDefinition parameter)
      {
         configuration.Generator.Generators.Add((parameterInfo, instructions, info) =>
         {
            ModuleDefinition moduleDefinition = ((MethodDefinition)parameter.Method).Module;
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
                parameterInfo.ParameterType == typeof(Object))
            {
               TypeReference reference = parameter.ParameterType;
               if (reference.IsByReference)
               {
                  reference =
                      ((MethodDefinition)parameter.Method).GenericParameters.First(
                          t => t.Name == reference.Name.TrimEnd('&'));
                  instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));
               }
               instructions.Add(Instruction.Create(OpCodes.Box, reference));
            }
         });
         return configuration;
      }

   }
}