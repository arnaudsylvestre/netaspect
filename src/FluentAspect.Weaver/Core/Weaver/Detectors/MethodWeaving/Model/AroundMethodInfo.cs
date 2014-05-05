using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine
{
    public class AroundMethodInfo : IAroundInfo
    {
        public ParametersIlGenerator<IlInjectorAvailableVariables> Generator { get; set; }
        public MethodDefinition Method { get; set; }
        public MethodInfo Interceptor { get; set; }
    }
}