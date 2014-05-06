using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
    public static class MethodWeavingPropertyGetInjectorFactory
    {
        public static IIlInjector<IlInjectorAvailableVariables> CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect)
        {
            FillCommon(method);


            return new Injector<IlInjectorAvailableVariables>(method, interceptorMethod, aspect, null);
        }

        public static IIlInjector<IlInjectorAvailableVariables> CreateForOnFinally(MethodDefinition method,
                                                                                   MethodInfo interceptorMethod,
                                                                                   NetAspectDefinition aspect)
        {
            FillCommon(method);

            return new Injector<IlInjectorAvailableVariables>(method, interceptorMethod, aspect, null);
        }

        private static void FillCommon(MethodDefinition method)
        {
            //parametersIlGenerator.CreateIlGeneratorForInstanceParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForPropertyParameter();
            //checker.CreateCheckerForInstanceParameter(method);
            //checker.CreateCheckerForMethodParameter();
            //checker.CreateCheckerForPropertyParameter();
        }

        public static IIlInjector<IlInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect)
        {
            FillCommon(method);
            //checker.CreateCheckerForResultParameter(method);
            return new Injector<IlInjectorAvailableVariables>(method, interceptorMethod, aspect, null);
        }

        public static IIlInjector<IlInjectorAvailableVariables> CreateForOnException(MethodDefinition method,
                                                                                     MethodInfo methodInfo,
                                                                                     NetAspectDefinition aspect)
        {
            FillCommon(method);
            //checker.CreateCheckerForExceptionParameter();

            return new Injector<IlInjectorAvailableVariables>(method, methodInfo, aspect, null);
        }
    }
}