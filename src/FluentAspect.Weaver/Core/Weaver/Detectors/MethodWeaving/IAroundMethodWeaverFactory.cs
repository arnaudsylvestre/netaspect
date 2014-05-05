using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public interface IAroundMethodWeaverFactory
    {
        IIlInjector<IlInjectorAvailableVariables> CreateForBefore(MethodDefinition method,
                                                                  MethodInfo interceptorMethod,
                                                                  NetAspectDefinition aspect);
        IIlInjector<IlInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                 MethodInfo interceptorMethod,
                                                                 NetAspectDefinition aspect);
        IIlInjector<IlInjectorAvailableVariables> CreateForExceptions(MethodDefinition method,
                                                                      MethodInfo interceptorMethod,
                                                                      NetAspectDefinition aspect);
        IIlInjector<IlInjectorAvailableVariables> CreateForOnFinally(MethodDefinition method,
                                                                     MethodInfo interceptorMethod,
                                                                     NetAspectDefinition aspect);
    }
}