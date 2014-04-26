using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public class InterceptorInfo
    {
        public ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> Generator { get; set; }
        public Instruction Instruction { get; set; }
        public MethodDefinition Method { get; set; }
        public MethodInfo Interceptor { get; set; }
    }
}