using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.V2;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class InstanceInterceptorParametersIlGenerator<T> : IInterceptorParameterIlGenerator<T>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
        }
    }
}