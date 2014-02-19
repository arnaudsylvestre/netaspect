using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods
{
   public class ParameterWeaver : IMethodWeaver
   {
      private MethodToWeave methodToWeave;
      private ParameterDefinition parameterDefinition;

      public ParameterWeaver(MethodToWeave methodToWeave_P, ParameterDefinition parameterDefinition_P)
      {
         methodToWeave = methodToWeave_P;
         parameterDefinition = parameterDefinition_P;
      }

      private Variables variables;
      private ParametersEngine beforeParametersEngine;
      private ParametersEngine afterParametersEngine;
      private ParametersEngine onExceptionParametersEngine;
      private ParametersEngine onFinallyParametersEngine;


      public void Init(Collection<VariableDefinition> variables, VariableDefinition result, ErrorHandler errorHandler)
      {
         this.variables = methodToWeave.CreateVariables();
         this.variables.handleResult = result;

         if (methodToWeave.Interceptors.HasBefore())
            beforeParametersEngine = ParametersEngineFactory.CreateForBeforeParameterWeaving(methodToWeave.Method.MethodDefinition, parameterDefinition, errorHandler);
         //if (methodToWeave.Interceptors.HasAfter())
         //   afterParametersEngine = ParametersEngineFactory.CreateForAfterMethodWeaving(methodToWeave.Method.MethodDefinition, this.variables.methodInfo, this.variables.args, this.variables.handleResult, errorHandler);
         //if (methodToWeave.Interceptors.HasOnException())
         //   onExceptionParametersEngine = ParametersEngineFactory.CreateForOnExceptionMethodWeaving(methodToWeave.Method.MethodDefinition, this.variables.methodInfo, this.variables.args, this.variables.exception, errorHandler);
         //if (methodToWeave.Interceptors.HasOnFinally())
         //   onFinallyParametersEngine = ParametersEngineFactory.CreateForOnFinallyMethodWeaving(methodToWeave.Method.MethodDefinition, this.variables.methodInfo, this.variables.args, errorHandler);
      }

      public void InsertBefore(Collection<Instruction> beforeInstructions)
      {
         Call(beforeInstructions, configuration_P => configuration_P.Before, beforeParametersEngine);
      }

      private void Call(Collection<Instruction> beforeInstructions, Func<MethodWeavingConfiguration, Interceptor> interceptorProvider, ParametersEngine parametersEngine)
      {
         for (int i = 0; i < methodToWeave.Interceptors.Count; i++)
         {
            var interceptorType = methodToWeave.Interceptors[i];
            var interceptorProvider_L = interceptorProvider(interceptorType);
            if (interceptorProvider_L.Method == null) continue;
            List<Instruction> instructions = new List<Instruction>();
            instructions.Add(Instruction.Create(OpCodes.Ldloc, variables.Interceptors[i]));
            parametersEngine.Fill(interceptorProvider_L.Method.GetParameters(), instructions);
            beforeInstructions.AddRange(instructions);
            beforeInstructions.Add(
               Instruction.Create(
                  OpCodes.Call,
                  methodToWeave.Method.MethodDefinition.Module.Import(
                     interceptorProvider_L
                        .Method)));
         }
      }

      public void InsertAfter(Collection<Instruction> afterInstructions)
      {
         Call(afterInstructions, configuration_P => configuration_P.After, afterParametersEngine);

      }

      

      public void InsertOnException(Collection<Instruction> onExceptionInstructions)
      {
         onExceptionInstructions.Add(Instruction.Create(OpCodes.Stloc, variables.exception));
         Call(onExceptionInstructions, configuration_P => configuration_P.OnException, onExceptionParametersEngine);
         onExceptionInstructions.Add(Instruction.Create(OpCodes.Rethrow));
      }

      public void InsertOnFinally(Collection<Instruction> onFinallyInstructions)
      {
         Call(onFinallyInstructions, configuration_P => configuration_P.OnFinally, onFinallyParametersEngine);
         onFinallyInstructions.Add(Instruction.Create(OpCodes.Endfinally));
      }

      public void CheckBefore(ErrorHandler errorHandlerPP)
      {
         Check(errorHandlerPP, configuration => configuration.Before.Method, beforeParametersEngine);
      }

      private void Check(ErrorHandler errorHandlerPP, Func<MethodWeavingConfiguration, MethodInfo> methodProvider, ParametersEngine parametersEngine)
      {
         foreach (var interceptor in methodToWeave.Interceptors)
         {
            if (methodProvider(interceptor) == null) continue;
            parametersEngine.Check(methodProvider(interceptor).GetParameters(), errorHandlerPP);
         }
      }

      public void CheckAfter(ErrorHandler errorHandler)
      {
         Check(errorHandler, configuration => configuration.After.Method, afterParametersEngine);
      }

      public void InsertInitInstructions(Collection<Instruction> initInstructions)
      {
         methodToWeave.InitializeVariables(variables, initInstructions);
      }

      public void CheckOnException(ErrorHandler errorHandler)
      {
         Check(errorHandler, configuration => configuration.OnException.Method, onExceptionParametersEngine);
      }

      public void CheckOnFinally(ErrorHandler errorHandler)
      {
         Check(errorHandler, configuration => configuration.OnFinally.Method, onFinallyParametersEngine);
      }
   }
}