using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
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
            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }

        public static IIlInjector CreateForOnFinally(MethodDefinition method,
                                                                                   MethodInfo interceptorMethod,
                                                                                   NetAspectDefinition aspect)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }

        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInjectorAvailableVariables> parametersIlGenerator)
        {
            parametersIlGenerator.CreateIlGeneratorForInstanceParameter(method);
            parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            parametersIlGenerator.CreateIlGeneratorForPropertyParameter();
            parametersIlGenerator.CreateIlGeneratorForPropertySetValueParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker)
        {
            checker.CreateCheckerForInstanceParameter(method);
            checker.CreateCheckerForMethodParameter();
            checker.CreateCheckerForPropertyParameter();
            checker.CreateCheckerForPropertySetValueParameter(method);
        }

        public static IIlInjector CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);

            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);
            return new MethodWeavingBeforeMethodInjector(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }

        public static IIlInjector CreateForOnException(MethodDefinition method,
                                                                                     MethodInfo methodInfo,
                                                                                     NetAspectDefinition aspect)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);
            checker.CreateCheckerForExceptionParameter();


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);
            parametersIlGenerator.CreateIlGeneratorForExceptionParameter();
            return new MethodWeavingBeforeMethodInjector(method, methodInfo,
                                                                                       checker, parametersIlGenerator, aspect);
        }
    }
}