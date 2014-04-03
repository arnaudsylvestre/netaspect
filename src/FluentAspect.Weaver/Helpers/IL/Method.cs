using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Helpers.IL
{
    public class Method
    {
        private readonly MethodDefinition definition;

        public Method(MethodDefinition definition)
        {
            this.definition = definition;
        }

        public MethodDefinition MethodDefinition
        {
            get { return definition; }
        }

        public void FillArgsArrayFromParameters(List<Instruction> instructions, VariableDefinition args)
        {
            if (args == null)
                return;
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4, definition.Parameters.Count));
            instructions.Add(Instruction.Create(OpCodes.Newarr, definition.Module.Import(typeof (object))));
            instructions.Add(Instruction.Create(OpCodes.Stloc, args));

            foreach (ParameterDefinition p in definition.Parameters.ToArray())
            {
                instructions.Add(Instruction.Create(OpCodes.Ldloc, args));
                instructions.Add(Instruction.Create(OpCodes.Ldc_I4, p.Index));
                instructions.Add(Instruction.Create(OpCodes.Ldarg, p));
                if (p.ParameterType.IsValueType || p.ParameterType is GenericParameter)
                    instructions.Add(Instruction.Create(OpCodes.Box, p.ParameterType));
                instructions.Add(Instruction.Create(OpCodes.Stelem_Ref));
            }
        }


        public void AddTryFinally(Instruction tryStart_L, Instruction tryEnd_L, Instruction handlerStart_L,
                                  Instruction handlerEnd_L)
        {
            MethodDefinition.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Finally)
                {
                    TryStart = tryStart_L,
                    TryEnd = tryEnd_L,
                    HandlerStart = handlerStart_L,
                    HandlerEnd = handlerEnd_L,
                });
        }

        public void AddTryCatch(Instruction tryStart_L, Instruction tryEnd_L, Instruction handlerStart_L,
                                Instruction handlerEnd_L)
        {
            MethodDefinition.Body.ExceptionHandlers.Add(new ExceptionHandler(ExceptionHandlerType.Catch)
                {
                    TryStart = tryStart_L,
                    TryEnd = tryEnd_L,
                    HandlerStart = handlerStart_L,
                    HandlerEnd = handlerEnd_L,
                    CatchType = MethodDefinition.Module.Import(typeof (Exception)),
                });
        }
    }

    
}