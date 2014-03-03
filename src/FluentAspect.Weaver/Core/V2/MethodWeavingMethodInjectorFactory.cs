using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public static class MethodWeavingMethodInjectorFactory
    {
        public static IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, Type aspectType)
        {

            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector(method, interceptorMethod, aspectType, checker, parametersIlGenerator);
        }
        public static IIlInjector CreateForOnFinally(MethodDefinition method, MethodInfo interceptorMethod, Type aspectType)
        {

            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector(method, interceptorMethod, aspectType, checker, parametersIlGenerator);
        }

        private static void FillCommon(MethodDefinition method, ParametersIlGenerator parametersIlGenerator)
        {
            parametersIlGenerator.CreateIlGeneratorForInstanceParameter(method);
            parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker)
        {
            checker.CreateCheckerForInstanceParameter(method);
            checker.CreateCheckerForMethodParameter();
            checker.CreateCheckerForParameterNameParameter(method);
            checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod, Type aspectType)
        {

            var checker = new ParametersChecker();
            FillCommon(method, checker);
            checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator();
            FillCommon(method, parametersIlGenerator);
            parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector(method, interceptorMethod, aspectType, checker, parametersIlGenerator);
        }

        public static IIlInjector CreateForOnException(MethodDefinition method, MethodInfo methodInfo, Type aspectType)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);
            checker.CreateCheckerForExceptionParameter();


            var parametersIlGenerator = new ParametersIlGenerator();
            FillCommon(method, parametersIlGenerator);
            parametersIlGenerator.CreateIlGeneratorForExceptionParameter();
            return new MethodWeavingBeforeMethodInjector(method, methodInfo, aspectType, checker, parametersIlGenerator);
        }
    }
}