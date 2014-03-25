using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Weaver.Checkers;
using FluentAspect.Weaver.Core.Weaver.Engine;
using FluentAspect.Weaver.Core.Weaver.Generators;
using FluentAspect.Weaver.Core.Weaver.Method;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weaver.Call
{
    public static class CallWeavingMethodInjectorFactory
    {
        public static IIlInjector<IlInstructionInjectorAvailableVariables> CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                Type aspectType)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator<IlInstructionInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector<IlInstructionInjectorAvailableVariables>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }
        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInstructionInjectorAvailableVariables> parametersIlGenerator)
        {
            parametersIlGenerator.CreateIlGeneratorForCallerParameter();
            //parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker)
        {
            checker.CreateCheckerForCallerParameter(method);
            //checker.CreateCheckerForMethodParameter();
            //checker.CreateCheckerForParameterNameParameter(method);
            //checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector<IlInstructionInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               Type aspectType)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);
            //checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator<IlInstructionInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);
            //parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector<IlInstructionInjectorAvailableVariables>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }
    }
}