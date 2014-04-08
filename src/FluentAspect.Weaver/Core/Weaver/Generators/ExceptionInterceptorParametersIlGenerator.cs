using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public class ExceptionInterceptorParametersIlGenerator :
        IInterceptorParameterIlGenerator<IlInjectorAvailableVariables>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions,
                               IlInjectorAvailableVariables info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.Exception));
        }
    }
}