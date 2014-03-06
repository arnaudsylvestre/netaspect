﻿using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Parameters
{
    public static class FieldParametersFactory
    {
        public static void AddFieldValue(this ParametersEngine engine, JoinPoint point, VariableDefinition value)
        {
            engine.AddPossibleParameter("value",
                                        (info, handler) => Ensure.ParameterType(info, handler, (point.InstructionStart.Operand as FieldReference).FieldType, null),
                                        (info, instructions) => instructions.Add(Instruction.Create(OpCodes.Ldloc, value)));
        }
    }
}