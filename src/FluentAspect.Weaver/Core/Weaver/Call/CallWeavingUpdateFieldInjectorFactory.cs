using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.Method;

namespace NetAspect.Weaver.Core.Weaver.Call
{
    public static class CallWeavingUpdateFieldInjectorFactory
    {
        public static IIlInjector<IlInstructionInjectorAvailableVariables> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, Type aspectType, Instruction instruction)
        {
            var calledField = (instruction.Operand as FieldReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledField, instruction);


            var parametersIlGenerator = new ParametersIlGenerator<IlInstructionInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator, instruction);

            return new MethodWeavingBeforeMethodInjector<IlInstructionInjectorAvailableVariables>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }
        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInstructionInjectorAvailableVariables> parametersIlGenerator, Instruction instruction)
        {
            parametersIlGenerator.CreateIlGeneratorForCalledParameter(instruction);
            parametersIlGenerator.CreateIlGeneratorForCallerParameter();
            parametersIlGenerator.CreateIlGeneratorForCallerParameters();
            parametersIlGenerator.CreateIlGeneratorForCallerParametersName(method);
            parametersIlGenerator.CreateIlGeneratorForColumnNumber(instruction);
            parametersIlGenerator.CreateIlGeneratorForLineNumber(instruction);
            parametersIlGenerator.CreateIlGeneratorForFilename(instruction);
            parametersIlGenerator.CreateIlGeneratorForFilePath(instruction);
            parametersIlGenerator.CreateIlGeneratorForField(instruction, method.Module);
            //parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker, FieldDefinition calledField, Instruction instruction)
        {
            //checker.CreateCheckerForCallerParameter(method);
            checker.CreateCheckerForCalledParameter(calledField);
            checker.CreateCheckerForCallerParameter(method);
            checker.CreateCheckerForCallerParameters(method);
            checker.CreateCheckerForCallerParametersName(method);
            checker.CreateCheckerForColumnNumberParameter(instruction);
            checker.CreateCheckerForLineNumberParameter(instruction);
            checker.CreateCheckerForFilenameParameter(instruction);
            checker.CreateCheckerForFilePathParameter(instruction);
            checker.CreateCheckerForField();
            //checker.CreateCheckerForParameterNameParameter(method);
            //checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector<IlInstructionInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               Type aspectType, Instruction instruction)
        {
            var calledField = (instruction.Operand as FieldReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledField, instruction);
            //checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator<IlInstructionInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator, instruction);
            //parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector<IlInstructionInjectorAvailableVariables>(method, interceptorMethod,
                                                                                       aspectType, checker,
                                                                                       parametersIlGenerator);
        }
    }
}