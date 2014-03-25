using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Generators
{
    public interface IInterceptorParameterIlGenerator<T>
    {
        void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info);
    }
}