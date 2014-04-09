using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call
{
    public static class CallWeavingMethodInjectorFactory
    {
        public static IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                Type aspectType, Instruction instruction)
        {
           var calledMethod = (instruction.Operand as MethodReference).Resolve();
           var checker = new ParametersChecker();
           FillCommon(method, checker, calledMethod);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, calledMethod);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }
        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> parametersIlGenerator, MethodDefinition calledMethod)
        {
            parametersIlGenerator.CreateIlGeneratorForCallerParameter();
            parametersIlGenerator.CreateIlGeneratorForCalledParameter();
            parametersIlGenerator.CreateIlGeneratorForCalledParameters();
            parametersIlGenerator.CreateIlGeneratorForCalledParametersName(calledMethod);
            parametersIlGenerator.CreateIlGeneratorForCallerParametersName(method);
            parametersIlGenerator.CreateIlGeneratorForCallerParameters();
            //parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker, MethodDefinition calledMethod)
        {
            checker.CreateCheckerForCallerParameter(method);
            checker.CreateCheckerForCalledParameter(method);
            checker.CreateCheckerForCalledParameters(method);
            checker.CreateCheckerForCalledParametersName(calledMethod);
            checker.CreateCheckerForCallerParametersName(method);
            checker.CreateCheckerForCallerParameters(method);
            //checker.CreateCheckerForMethodParameter();
            //checker.CreateCheckerForParameterNameParameter(method);
            //checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               Type aspectType, Instruction instruction)
        {
           var calledMethod = (instruction.Operand as MethodReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledMethod);
            //checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, calledMethod);
            //parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }
    }
}