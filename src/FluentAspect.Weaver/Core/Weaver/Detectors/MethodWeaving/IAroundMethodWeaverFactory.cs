using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public interface IAroundMethodWeaverFactory
    {
        IIlInjector CreateForBefore(MethodDefinition method,
                                                                  MethodInfo interceptorMethod,
                                                                  NetAspectDefinition aspect);
        IIlInjector CreateForAfter(MethodDefinition method,
                                                                 MethodInfo interceptorMethod,
                                                                 NetAspectDefinition aspect);
        IIlInjector CreateForExceptions(MethodDefinition method,
                                                                      MethodInfo interceptorMethod,
                                                                      NetAspectDefinition aspect);
        IIlInjector CreateForOnFinally(MethodDefinition method,
                                                                     MethodInfo interceptorMethod,
                                                                     NetAspectDefinition aspect);
    }
}