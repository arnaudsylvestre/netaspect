using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weaver.Generators
{
    public interface IInterceptorParameterIlGenerator<T>
    {
        void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info);
    }
}