using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.V2.Weaver.Call;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Generators
{
    public class CalledInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInstructionInjectorAvailableVariables>
    {
        private Instruction instruction;

        public CalledInterceptorParametersIlGenerator(Instruction instruction)
        {
            this.instruction = instruction;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInstructionInjectorAvailableVariables info)
        {
            if (!info.VariablesCalled.ContainsKey(instruction))
            {
                instructions.Add(Instruction.Create(OpCodes.Ldnull));
            }
            else
                instructions.Add(Instruction.Create(OpCodes.Ldloc, info.VariablesCalled[instruction]));
        }
    }
}