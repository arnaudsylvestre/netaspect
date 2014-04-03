using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Call;

namespace NetAspect.Weaver.Core.Weaver.Generators
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