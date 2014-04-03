using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.Helpers
{
    public static class InstructionsHelper
    {
        public static List<Instruction> FixReturns(MethodDefinition method, VariableDefinition handleResultP_P,
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