using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods
{
    public interface IMethodWeaver
    {
       void Init(Collection<VariableDefinition> variables, VariableDefinition result, ErrorHandler errorHandler);
       void InsertBefore(Collection<Instruction> method);
       void InsertAfter(Collection<Instruction> afterInstructions);
       void InsertOnException(Collection<Instruction> onExceptionInstructions);
       void InsertOnFinally(Collection<Instruction> onFinallyInstructions);
        void CheckBefore(ErrorHandler errorHandlerPP);
        void CheckAfter(ErrorHandler errorHandler);
        void InsertInitInstructions(Collection<Instruction> initInstructions);
    }

   public class MethodWeaver : IMethodWeaver
   {
      private MethodToWeave methodToWeave;

      public MethodWeaver(MethodToWeave methodToWeave_P)
      {
         methodToWeave = methodToWeave_P;
      }

      private Variables variables;
      private ParametersEngine beforeParametersEngine;
      private ParametersEngine afterParametersEngine;


      public void Init(Collection<VariableDefinition> variables, VariableDefinition result, ErrorHandler errorHandler)
      {
         this.variables = methodToWeave.CreateVariables();
          this.variables.handleResult = result;

         if (methodToWeave.Interceptors.HasBefore())
            beforeParametersEngine = ParametersEngineFactory.CreateForBeforeMethodWeaving(methodToWeave.Method.MethodDefinition, this.variables.methodInfo, this.variables.args, errorHandler);
         if (methodToWeave.Interceptors.HasAfter())
          afterParametersEngine = ParametersEngineFactory.CreateForAfterMethodWeaving(methodToWeave.Method.MethodDefinition, this.variables.methodInfo, this.variables.args, this.variables.handleResult, errorHandler);
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
          //methodToWeave.GenerateOnExceptionInterceptor(variables, onExceptionInstructions);
      }

      public void InsertOnFinally(Collection<Instruction> onFinallyInstructions)
      {
         //onFinallyInstructions.Add(Instruction.Create(OpCodes.Nop));
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
   }


    public class NewAroundMethodWeaver
    {
        private MethodToWeave method;
        private IMethodWeaver methodWeaver;
       private VariableDefinition result;

       public NewAroundMethodWeaver(MethodToWeave method, IMethodWeaver methodWeaver)
        {
            this.method = method;
            this.methodWeaver = methodWeaver;
        }

        public void Weave()
        {
            var befores = from b in method.Interceptors where b.Before.Method != null select b;
            var afters = from b in method.Interceptors where b.After.Method != null select b;
            var onExceptions = from b in method.Interceptors where b.OnException.Method != null select b;
            if (!befores.Any() && !afters.Any() && !onExceptions.Any())
            {
                return;
            }
            method.Method.MethodDefinition.Body.InitLocals = true;
            var initInstructions = new Collection<Instruction>();
            var beforeInstructions = new Collection<Instruction>();
            var beforeAfter = Instruction.Create(OpCodes.Nop);
            var afterInstructions = new Collection<Instruction>();
            var onExceptionInstructions = new Collection<Instruction>();
            var onFinallyInstructions = new Collection<Instruction>();
            methodWeaver.InsertInitInstructions(initInstructions);
            methodWeaver.InsertBefore(beforeInstructions);
            methodWeaver.InsertOnException(onExceptionInstructions);
            methodWeaver.InsertAfter(afterInstructions);
           methodWeaver.InsertOnFinally(onFinallyInstructions);
            var methodInstructions = new Collection<Instruction>(method.Method.MethodDefinition.Body.Instructions);
            var end = FixReturns(method.Method.MethodDefinition, result, methodInstructions, beforeAfter);
            var allInstructions = new List<Instruction>();
            allInstructions.AddRange(initInstructions);
            allInstructions.AddRange(beforeInstructions);
            allInstructions.AddRange(methodInstructions);
            allInstructions.Add(beforeAfter);
            allInstructions.AddRange(afterInstructions);
            allInstructions.AddRange(end);


            
            //allInstructions.AddRange(onExceptionInstructions);
            //allInstructions.AddRange(onFinallyInstructions);

           method.Method.MethodDefinition.Body.Instructions.Clear();
           method.Method.MethodDefinition.Body.Instructions.AddRange(allInstructions);


           // if (onExceptions.Any())
           //method.Method.AddTryCatch(first, last, onExceptionInstructions.First(), onExceptionInstructions.Last());
           // if (onFinallyInstructions.Any())
           //method.Method.AddTryFinally(first, last, onFinallyInstructions.First(), onFinallyInstructions.Last());

        }


        List<Instruction> FixReturns(MethodDefinition method, VariableDefinition handleResultP_P, Collection<Instruction> instructions, Instruction beforeAfter)
        {
            List<Instruction> end = new List<Instruction>();
            if (method.ReturnType == method.Module.TypeSystem.Void)
            {
                end.Add(Instruction.Create(OpCodes.Ret));

                for (var index = 0; index < instructions.Count; index++)
                {
                    var instruction = instructions[index];
                    if (instruction.OpCode == OpCodes.Ret)
                    {
                        instructions[index] = Instruction.Create(OpCodes.Leave, beforeAfter);
                    }
                }
            }
            else
            {
                end.Add(Instruction.Create(OpCodes.Ldloc, handleResultP_P));
                        end.Add(Instruction.Create(OpCodes.Ret));

                for (var index = 0; index < instructions.Count; index++)
                {
                    var instruction = instructions[index];
                    if (instruction.OpCode == OpCodes.Ret)
                    {
                        instructions[index] = Instruction.Create(OpCodes.Leave, beforeAfter);
                        instructions.Insert(index, Instruction.Create(OpCodes.Stloc, handleResultP_P));
                        index++;
                    }
                }
            }
            return end;
        }

       public void Check(ErrorHandler errorHandlerP_P)
       {
           if (method.Method.MethodDefinition.ReturnType != method.Method.MethodDefinition.Module.TypeSystem.Void)
               result = method.Method.MethodDefinition.CreateVariable(method.Method.MethodDefinition.ReturnType);
           methodWeaver.Init(method.Method.MethodDefinition.Body.Variables, result, errorHandlerP_P);
          methodWeaver.CheckBefore(errorHandlerP_P);
          //methodWeaver.CheckOnException(errorHandlerP_P);
          methodWeaver.CheckAfter(errorHandlerP_P);
          //methodWeaver.CheckOnFinally(errorHandlerP_P);
       }
    }


}