using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Weaver.Method;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weaver.Generators
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