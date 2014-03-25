using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.V2.Weaver.Method;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Generators
{
    public class ResultInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInjectorAvailableVariables>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions,
                               IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(parameterInfo.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc,
                                                info.Result));
        }
    }
}