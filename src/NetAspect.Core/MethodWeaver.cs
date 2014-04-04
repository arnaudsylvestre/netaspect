using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core.Helpers;

namespace NetAspect.Core
{
    public static class MethodWeaver
    {


        public static void AddRange(this IList<Instruction> instructions, IEnumerable<Instruction> toAdd)
        {
            foreach (Instruction instruction in toAdd)
            {
                instructions.Add(instruction);
            }
        }

        public static void Weave(this MethodDefinition method, NetAspectWeavingMethod weavingModel)
        {
            WeaveInstructions(method, weavingModel.Instructions);
            

            var result = method.ReturnType == method.Module.TypeSystem.Void ? null : new VariableDefinition(method.ReturnType);

            if (result != null)
                method.Body.Variables.Add(result);
            var allInstructions = new List<Instruction>();
            WeaveConstructorInstructions(method, weavingModel, allInstructions);
            var methodInstructions = method.ExtractRealInstructions();

            if (methodInstructions.Count == 0)
                methodInstructions.Add(Instruction.Create(OpCodes.Nop));

            Instruction beforeAfter = Instruction.Create(OpCodes.Nop);
            List<Instruction> end = method.FixReturns(result, methodInstructions, beforeAfter);

            allInstructions.AddRange(weavingModel.BeforeInstructions);
            allInstructions.AddRange(methodInstructions);
            if (end.Count != 0)
            {
                allInstructions.Add(beforeAfter);
                allInstructions.AddRange(weavingModel.AfterInstructions);
                Instruction gotoEnd = Instruction.Create(OpCodes.Leave, end.First());
                allInstructions.Add(gotoEnd);
            }


            if (weavingModel.OnExceptionInstructions.Any())
            {
                Instruction beforeExceptionManagementInstruction = Instruction.Create(OpCodes.Nop);
                Instruction afterExceptionManagementInstructions = end.Count > 0 ? Instruction.Create(OpCodes.Leave, end.First()) : Instruction.Create(OpCodes.Nop);
                allInstructions.Add(beforeExceptionManagementInstruction);
                allInstructions.AddRange(weavingModel.OnExceptionInstructions);
                allInstructions.Add(afterExceptionManagementInstructions);

                Instruction lastCatchException = null;
                if (weavingModel.OnFinallyInstructions.Any())
                    lastCatchException = weavingModel.OnFinallyInstructions.First();
                else
                {
                    if (end.Count != 0)
                        lastCatchException = end.First();
                }

                method.AddTryCatch(methodInstructions.First(), beforeExceptionManagementInstruction,
                                   beforeExceptionManagementInstruction, lastCatchException);
            }

            if (weavingModel.OnFinallyInstructions.Any())
            {

                if (end.Count > 0)
                    weavingModel.OnFinallyInstructions.Add(Instruction.Create(OpCodes.Endfinally));
                allInstructions.AddRange(weavingModel.OnFinallyInstructions);
                method.AddTryFinally(methodInstructions.First(), weavingModel.OnFinallyInstructions.First(),
                                     weavingModel.OnFinallyInstructions.First(), end.Count > 0 ? end.First() : null);

            }
            allInstructions.AddRange(end);
                

            method.Body.Instructions.Clear();
            method.Body.Instructions.AddRange(allInstructions);
            Finalize(method);
        }

        private static void WeaveConstructorInstructions(MethodDefinition method, NetAspectWeavingMethod weavingModel,
                                                         List<Instruction> allInstructions)
        {
            if (method.IsConstructor)
            {
                allInstructions.AddRange(weavingModel.BeforeConstructorBaseCall);
                allInstructions.AddRange(method.Body.Instructions.ExtractBeforeCallBaseConstructorInstructions());
                allInstructions.Add(method.Body.Instructions.GetCallBaseConstructorInstructions());
            }
        }


        private static void Finalize(MethodDefinition methodDefinition)
        {
            methodDefinition.Body.InitLocals = methodDefinition.Body.HasVariables;
        }

        private static void WeaveInstructions(MethodDefinition method, Dictionary<Instruction, NetAspectWeavingMethod.InstructionIl> instructions)
        {
            var allInstructions = new List<Instruction>();
            foreach (var instruction in method.Body.Instructions)
            {
                
                if (!instructions.ContainsKey(instruction))
                {
                    allInstructions.Add(instruction);
                    continue;
                }
                var instructionIl = instructions[instruction];
                allInstructions.AddRange(instructionIl.Before);
                allInstructions.Add(instruction);
                allInstructions.AddRange(instructionIl.After);
            }
            method.Body.Instructions.Clear();
            method.Body.Instructions.AddRange(allInstructions);
        }
    }
}