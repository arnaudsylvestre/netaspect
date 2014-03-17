using System;
using System.Reflection;
using FluentAspect.Weaver.Core.V2.Weaver.Checkers;
using FluentAspect.Weaver.Core.V2.Weaver.Engine;
using FluentAspect.Weaver.Core.V2.Weaver.Generators;
using FluentAspect.Weaver.Core.V2.Weaver.Method;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Call
{
    public static class CallWeavingEventInjectorFactory
    {
        public static IIlInjector<IlInstructionInjectorAvailableVariables> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, Type aspectType, Instruction instruction)
        {
            var calledMethod = (instruction.Next.Operand as MethodReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledMethod);


            var parametersIlGenerator = new ParametersIlGenerator<IlInstructionInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector<IlInstructionInjectorAvailableVariables>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }
        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInstructionInjectorAvailableVariables> parametersIlGenerator)
        {
            parametersIlGenerator.CreateIlGeneratorForCallerParameter(method);
            parametersIlGenerator.CreateIlGeneratorForCalledParametersName(method);
            //parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker, MethodDefinition calledMethod)
        {
            checker.CreateCheckerForCallerParameter(method);
            checker.CreateCheckerForCalledParametersName(calledMethod);
            //checker.CreateCheckerForMethodParameter();
            //checker.CreateCheckerForParameterNameParameter(method);
            //checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector<IlInstructionInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               Type aspectType, Instruction instruction)
        {
            var calledMethod = (instruction.Operand as MethodReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledMethod);
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