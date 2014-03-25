using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Weaver.Method;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weaver.Generators
{
    public class PropertyInterceptorParametersIlGenerator :
        IInterceptorParameterIlGenerator<IlInjectorAvailableVariables>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions,
                               IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentPropertyInfo));
        }
    }
}