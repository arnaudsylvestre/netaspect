using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.V2;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public interface IInterceptorParameterIlGenerator
    {
        void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info);
    }
}