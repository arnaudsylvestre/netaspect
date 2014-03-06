using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods
{
    public class NewAroundMethodWeaver
    {
        private readonly MethodToWeave method;
        private readonly IMethodWeaver methodWeaver;
        private VariableDefinition result;

        public NewAroundMethodWeaver(MethodToWeave method, IMethodWeaver methodWeaver)
        {
            this.method = method;
            this.methodWeaver = methodWeaver;
        }

        public void Weave()
        {
            IEnumerable<MethodWeavingConfiguration> befores = from b in method.Interceptors
                                                              where b.Before.Method != null
                                                              select b;
            IEnumerable<MethodWeavingConfiguration> afters = from b in method.Interceptors
                                                             where b.After.Method != null
                                                             select b;
            IEnumerable<MethodWeavingConfiguration> onExceptions = from b in method.Interceptors
                                                                   where b.OnException.Method != null
                                                                   select b;
            IEnumerable<MethodWeavingConfiguration> onFinallys = from b in method.Interceptors
                                                                 where b.OnFinally.Method != null
                                                                 select b;
            if (!befores.Any() && !afters.Any() && !onExceptions.Any() && !onFinallys.Any())
            {
                return;
            }
            method.Method.MethodDefinition.Body.InitLocals = true;
            var initInstructions = new Collection<Instruction>();
            var beforeInstructions = new Collection<Instruction>();
            Instruction beforeAfter = Instruction.Create(OpCodes.Nop);
            var afterInstructions = new Collection<Instruction>();
            var onExceptionInstructions = new Collection<Instruction>();
            var onFinallyInstructions = new Collection<Instruction>();
            methodWeaver.InsertInitInstructions(initInstructions);
            if (befores.Any())
                methodWeaver.InsertBefore(beforeInstructions);
            if (onExceptions.Any())
                methodWeaver.InsertOnException(onExceptionInstructions);
            if (afters.Any())
                methodWeaver.InsertAfter(afterInstructions);
            if (onFinallys.Any())
                methodWeaver.InsertOnFinally(onFinallyInstructions);
            var methodInstructions = new Collection<Instruction>(method.Method.MethodDefinition.Body.Instructions);
            List<Instruction> end = FixReturns(method.Method.MethodDefinition, result, methodInstructions, beforeAfter);
            var allInstructions = new List<Instruction>();
            allInstructions.AddRange(initInstructions);
            allInstructions.AddRange(beforeInstructions);
            allInstructions.AddRange(methodInstructions);
            if (end.Count != 0)
            {
                allInstructions.Add(beforeAfter);
                allInstructions.AddRange(afterInstructions);
                Instruction gotoEnd = Instruction.Create(OpCodes.Leave, end.First());
                allInstructions.Add(gotoEnd);
            }


            allInstructions.AddRange(onExceptionInstructions);
            allInstructions.AddRange(onFinallyInstructions);
            allInstructions.AddRange(end);

            //if (end.Count == 0)
            //{
            //   allInstructions.Add(Instruction.Create(OpCodes.Ret));
            //}
            method.Method.MethodDefinition.Body.Instructions.Clear();
            method.Method.MethodDefinition.Body.Instructions.AddRange(allInstructions);


            if (onExceptionInstructions.Any())
            {
                Instruction lastCatchException = null;
                if (onFinallyInstructions.Any())
                    lastCatchException = onFinallyInstructions.First();
                else
                {
                    if (end.Count != 0)
                        lastCatchException = end.First();
                }

                method.Method.AddTryCatch(methodInstructions.First(), onExceptionInstructions.First(),
                                          onExceptionInstructions.First(), lastCatchException);
            }

            if (onFinallyInstructions.Any())
                method.Method.AddTryFinally(methodInstructions.First(), onFinallyInstructions.First(),
                                            onFinallyInstructions.First(), end.Count > 0 ? end.First() : null);
        }


        private List<Instruction> FixReturns(MethodDefinition method, VariableDefinition handleResultP_P,
                                             Collection<Instruction> instructions, Instruction beforeAfter)
        {
            var end = new List<Instruction>();
            if (method.ReturnType == method.Module.TypeSystem.Void)
            {
                for (int index = 0; index < instructions.Count; index++)
                {
                    Instruction instruction = instructions[index];
                    if (instruction.OpCode == OpCodes.Ret)
                    {
                        if (end.Count == 0)
                            end.Add(Instruction.Create(OpCodes.Ret));

                        instructions[index] = Instruction.Create(OpCodes.Leave, beforeAfter);
                    }
                }
            }
            else
            {
                for (int index = 0; index < instructions.Count; index++)
                {
                    Instruction instruction = instructions[index];
                    if (instruction.OpCode == OpCodes.Ret)
                    {
                        if (end.Count == 0)
                        {
                            end.Add(Instruction.Create(OpCodes.Ldloc, handleResultP_P));
                            end.Add(Instruction.Create(OpCodes.Ret));
                        }
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
            methodWeaver.CheckOnException(errorHandlerP_P);
            methodWeaver.CheckAfter(errorHandlerP_P);
            methodWeaver.CheckOnFinally(errorHandlerP_P);
        }
    }
}