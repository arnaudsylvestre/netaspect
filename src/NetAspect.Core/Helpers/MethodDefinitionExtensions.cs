using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Core.Helpers
{
    public static class MethodDefinitionExtensions
    {
        public static List<Instruction> ExtractRealInstructions(this MethodDefinition method)
        {
            if (!method.IsConstructor)
                return new List<Instruction>(method.Body.Instructions);
            var allInstructions = new List<Instruction>();
            bool callBaseContructorFound = false;
            for (int i = 0; i < method.Body.Instructions.Count; i++)
            {
                var instruction = method.Body.Instructions[i];
                if (callBaseContructorFound)
                    allInstructions.Add(instruction);
                if (instruction.IsCallBaseConstructor())
                {
                    callBaseContructorFound = true;
                }
            }
            return allInstructions;
        }

        public static void AddTryFinally(this MethodDefinition methodDefinition, Instruction tryStart_L, Instruction tryEnd_L, Instruction handlerStart_L,
                                         Instruction handlerEnd_L)
        {
            methodDefinition.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
            {
                TryStart = tryStart_L,
                TryEnd = tryEnd_L,
                HandlerStart = handlerStart_L,
                HandlerEnd = handlerEnd_L,
            });
        }

        public static void AddTryCatch(this MethodDefinition methodDefinition, Instruction tryStart_L, Instruction tryEnd_L, Instruction handlerStart_L,
                                       Instruction handlerEnd_L)
        {
            methodDefinition.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Catch)
            {
                TryStart = tryStart_L,
                TryEnd = tryEnd_L,
                HandlerStart = handlerStart_L,
                HandlerEnd = handlerEnd_L,
                CatchType = methodDefinition.Module.Import(typeof(Exception)),
            });
        }

        public static List<Instruction> FixReturns(this MethodDefinition method, VariableDefinition handleResultP_P,
                                                   List<Instruction> instructions, Instruction beforeAfter)
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
    }
}