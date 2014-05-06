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
    public static class MethodWeavingPropertySetInjectorFactory
    {
       public static IIlInjector CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect)
        {
            FillCommon(method);

            return new Injector(method, interceptorMethod
                                                                                       , aspect, null);
        }

       public static IIlInjector CreateForOnFinally(MethodDefinition method,
                                                                                   MethodInfo interceptorMethod,
                                                                                   NetAspectDefinition aspect)
        {
            FillCommon(method);

            return new Injector(method, interceptorMethod, aspect, null);
        }

        private static void FillCommon(MethodDefinition method)
        {
            //parametersIlGenerator.CreateIlGeneratorForInstanceParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForPropertyParameter();
            //parametersIlGenerator.CreateIlGeneratorForPropertySetValueParameter(method);
            //checker.CreateCheckerForInstanceParameter(method);
            //checker.CreateCheckerForMethodParameter();
            //checker.CreateCheckerForPropertyParameter();
            //checker.CreateCheckerForPropertySetValueParameter(method);
        }

        public static IIlInjector CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect)
        {
            FillCommon(method);

            return new Injector(method, interceptorMethod, aspect, null);
        }

        public static IIlInjector CreateForOnException(MethodDefinition method,
                                                                                     MethodInfo methodInfo,
                                                                                     NetAspectDefinition aspect)
        {
           FillCommon(method);
            //parametersIlGenerator.CreateIlGeneratorForExceptionParameter();
            return new Injector(method, methodInfo, aspect, null);
        }
    }
}