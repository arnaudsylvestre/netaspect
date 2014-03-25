using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Weaver.Call;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weaver.Generators
{
    public class ParametersInterceptorParametersIlGenerator<T> :
        IInterceptorParameterIlGenerator<T>
        where T : IlInstructionInjectorAvailableVariables
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions,
                               T info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.Parameters));
        }
    }
}