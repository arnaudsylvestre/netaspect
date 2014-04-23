using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call
{
    public static class CallWeavingMethodInjectorFactory
    {
        public static IIlInjector CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect, Instruction instruction)
        {
           var calledMethod = (instruction.Operand as MethodReference).Resolve();
           var checker = new ParametersChecker();
           FillCommon(method, checker, calledMethod, instruction);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, calledMethod, instruction);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> parametersIlGenerator, MethodDefinition calledMethod, Instruction instruction)
        {
            parametersIlGenerator.CreateIlGeneratorForCallerParameter();
            parametersIlGenerator.CreateIlGeneratorForCalledParameter();
            parametersIlGenerator.CreateIlGeneratorForCalledParameters();
            parametersIlGenerator.CreateIlGeneratorForCalledParametersName(calledMethod);
            parametersIlGenerator.CreateIlGeneratorForCallerParametersName(method);
            parametersIlGenerator.CreateIlGeneratorForCallerParameters();
            parametersIlGenerator.CreateIlGeneratorForColumnNumber(instruction);
            parametersIlGenerator.CreateIlGeneratorForLineNumber(instruction);
            parametersIlGenerator.CreateIlGeneratorForFilename(instruction);
            parametersIlGenerator.CreateIlGeneratorForFilePath(instruction);
            parametersIlGenerator.CreateIlGeneratorForCallerMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker, MethodDefinition calledMethod, Instruction instruction)
        {
            checker.CreateCheckerForCallerParameter(method);
            checker.CreateCheckerForCalledParameter(method);
            checker.CreateCheckerForCalledParameters(method);
            checker.CreateCheckerForCalledParametersName(calledMethod);
            checker.CreateCheckerForCallerParametersName(method);
            checker.CreateCheckerForCallerParameters(method);
            checker.CreateCheckerForColumnNumberParameter(instruction);
            checker.CreateCheckerForLineNumberParameter(instruction);
            checker.CreateCheckerForFilenameParameter(instruction);
            checker.CreateCheckerForFilePathParameter(instruction);
            checker.CreateCheckerForCallerMethodParameter();
            //checker.CreateCheckerForMethodParameter();
            //checker.CreateCheckerForParameterNameParameter(method);
            //checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect, Instruction instruction)
        {
           var calledMethod = (instruction.Operand as MethodReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledMethod, instruction);
            //checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, calledMethod, instruction);
            //parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
    }
}