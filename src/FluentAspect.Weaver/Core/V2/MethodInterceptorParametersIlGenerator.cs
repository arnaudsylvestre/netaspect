using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.V2;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class MethodInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInjectorAvailableVariables>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentMethodBase));
        }
    }
    public class PropertyInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInjectorAvailableVariables>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CurrentPropertyInfo));
        }
    }
}