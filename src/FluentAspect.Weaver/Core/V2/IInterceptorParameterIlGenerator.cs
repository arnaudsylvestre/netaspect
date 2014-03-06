using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public interface IInterceptorParameterIlGenerator<T>
    {
        void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info);
    }
}