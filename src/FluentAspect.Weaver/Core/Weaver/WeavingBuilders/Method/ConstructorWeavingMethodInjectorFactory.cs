﻿using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
    public static class ConstructorWeavingMethodInjectorFactory
    {
        public static IIlInjector<IlInjectorAvailableVariables> CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                Type aspectType)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariables>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }

        public static IIlInjector<IlInjectorAvailableVariables> CreateForOnFinally(MethodDefinition method,
                                                                                   MethodInfo interceptorMethod,
                                                                                   Type aspectType)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariables>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }

        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInjectorAvailableVariables> parametersIlGenerator)
        {
            parametersIlGenerator.CreateIlGeneratorForInstanceParameter(method);
            parametersIlGenerator.CreateIlGeneratorForConstructorParameter();
            parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker)
        {
            checker.CreateCheckerForInstanceParameter(method);
            checker.CreateCheckerForConstructorParameter();
            checker.CreateCheckerForParameterNameParameter(method);
            checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector<IlInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               Type aspectType)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);
            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariables>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }

        public static IIlInjector<IlInjectorAvailableVariables> CreateForOnException(MethodDefinition method,
                                                                                     MethodInfo methodInfo,
                                                                                     Type aspectType)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);
            checker.CreateCheckerForExceptionParameter();


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);
            parametersIlGenerator.CreateIlGeneratorForExceptionParameter();
            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariables>(method, methodInfo, aspectType,
                                                                                       checker, parametersIlGenerator);
        }
    }
}