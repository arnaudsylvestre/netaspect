using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods
{
    public interface IMethodWeaver
    {
       void Init(Collection<Instruction> initInstructions, Collection<VariableDefinition> variables);
       void InsertBefore(Collection<Instruction> method);
       void InsertAfter(Collection<Instruction> afterInstructions);
       void InsertOnException(Collection<Instruction> onExceptionInstructions);
       void InsertOnFinally(Collection<Instruction> onFinallyInstructions);
    }

   public class MethodWeaver : IMethodWeaver
   {
      private MethodToWeave methodToWeave;

      public MethodWeaver(MethodToWeave methodToWeave_P)
      {
         methodToWeave = methodToWeave_P;
      }

      private Variables variables;


      public void Init(Collection<Instruction> initInstructions, Collection<VariableDefinition> variables)
      {
         this.variables = methodToWeave.CreateVariables(variables, initInstructions);
      }

      public void InsertBefore(Collection<Instruction> beforeInstructions)
      {
          methodToWeave.CallBefore(variables, beforeInstructions);
      }

      public void InsertAfter(Collection<Instruction> afterInstructions)
      {
          methodToWeave.CallAfter(variables, afterInstructions);
      }

      public void InsertOnException(Collection<Instruction> onExceptionInstructions)
      {
          methodToWeave.GenerateOnExceptionInterceptor(variables, onExceptionInstructions);
      }

      public void InsertOnFinally(Collection<Instruction> onFinallyInstructions)
      {
         onFinallyInstructions.Add(Instruction.Create(OpCodes.Nop));
      }
   }


    public class NewAroundMethodWeaver
    {
        private MethodToWeave method;
        private IMethodWeaver methodWeaver;

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


            var initInstructions = new Collection<Instruction>();
            var beforeInstructions = new Collection<Instruction>();
            var afterInstructions = new Collection<Instruction>();
            var onExceptionInstructions = new Collection<Instruction>();
            var onFinallyInstructions = new Collection<Instruction>();
            Variables variables = method.CreateVariables(method.Method.MethodDefinition.Body.Variables, initInstructions);
            methodWeaver.Init(initInstructions, method.Method.MethodDefinition.Body.Variables);
            methodWeaver.InsertBefore(beforeInstructions);
            methodWeaver.InsertOnException(onExceptionInstructions);
            methodWeaver.InsertAfter(afterInstructions);
           methodWeaver.InsertOnFinally(onFinallyInstructions);
           FixReturns(method.Method.MethodDefinition, variables.handleResult);
            var first = method.Method.MethodDefinition.Body.Instructions.First();
            var last = method.Method.MethodDefinition.Body.Instructions.Last();
            var allInstructions = new List<Instruction>();
            allInstructions.AddRange(beforeInstructions);
            allInstructions.AddRange(method.Method.MethodDefinition.Body.Instructions);
            allInstructions.AddRange(afterInstructions);


            method.Method.Return(variables.handleResult, afterInstructions);
            
            allInstructions.AddRange(onExceptionInstructions);
            allInstructions.AddRange(onFinallyInstructions);

           method.Method.MethodDefinition.Body.Instructions.Clear();
           method.Method.MethodDefinition.Body.Instructions.AddRange(allInstructions);

           method.Method.AddTryCatch(first, last, onExceptionInstructions.First(), onExceptionInstructions.Last());
           method.Method.AddTryFinally(first, last, onFinallyInstructions.First(), onFinallyInstructions.Last());

        }

        void FixReturns(MethodDefinition method, VariableDefinition handleResultP_P)
        {
           if (method.ReturnType == method.Module.TypeSystem.Void)
           {
              var instructions = method.Body.Instructions;

              for (var index = 0; index < instructions.Count - 1; index++)
              {
                 var instruction = instructions[index];
                 if (instruction.OpCode == OpCodes.Ret)
                 {
                    instructions[index] = Instruction.Create(OpCodes.Leave, Instruction.Create(OpCodes.Ret));
                 }
              }
           }
           else
           {
              var instructions = method.Body.Instructions;

              for (var index = 0; index < instructions.Count - 2; index++)
              {
                 var instruction = instructions[index];
                 if (instruction.OpCode == OpCodes.Ret)
                 {
                    instructions[index] = Instruction.Create(OpCodes.Leave, Instruction.Create(OpCodes.Ldloc, handleResultP_P));
                    instructions.Insert(index, Instruction.Create(OpCodes.Stloc, handleResultP_P));
                    index++;
                 }
              }
           }
        }


    }


}