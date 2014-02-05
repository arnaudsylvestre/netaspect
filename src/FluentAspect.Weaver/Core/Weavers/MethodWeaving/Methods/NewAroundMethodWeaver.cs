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
       void Init(Collection<Instruction> initInstructions, Collection<VariableDefinition> variables, VariableDefinition result);
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


      public void Init(Collection<Instruction> initInstructions, Collection<VariableDefinition> variables, VariableDefinition result)
      {
         this.variables = methodToWeave.CreateVariables(variables, initInstructions);
          this.variables.handleResult = result;
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
            method.Method.MethodDefinition.Body.InitLocals = true;
            var returnType = method.Method.MethodDefinition.ReturnType;
            var result = returnType == method.Method.MethodDefinition.Module.TypeSystem.Void ? null : method.Method.MethodDefinition.CreateVariable(returnType);
            var initInstructions = new Collection<Instruction>();
            var beforeInstructions = new Collection<Instruction>();
            var beforeAfter = Instruction.Create(OpCodes.Nop);
            var afterInstructions = new Collection<Instruction>();
            var onExceptionInstructions = new Collection<Instruction>();
            var onFinallyInstructions = new Collection<Instruction>();
            methodWeaver.Init(initInstructions, method.Method.MethodDefinition.Body.Variables, result);
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

    }


}