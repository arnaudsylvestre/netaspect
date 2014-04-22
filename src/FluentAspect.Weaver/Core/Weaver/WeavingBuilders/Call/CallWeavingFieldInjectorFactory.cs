﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call
{
    public class NoIIlInjector : IIlInjector<IlInjectorAvailableVariablesForInstruction>
    {
        public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction availableInformations)
        {
            
        }

        public void Inject(List<Instruction> instructions, IlInjectorAvailableVariablesForInstruction availableInformations)
        {
            
        }
    }

    public static class CallWeavingFieldInjectorFactory
    {
        public static IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction)
        {
            var calledField = (instruction.Operand as FieldReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledField, instruction);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, instruction);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> parametersIlGenerator, Instruction instruction)
        {
            parametersIlGenerator.CreateIlGeneratorForCalledParameter();
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

        public static IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect, Instruction instruction)
        {
            var calledField = (instruction.Operand as FieldReference).Resolve();
            var checker = new ParametersChecker();
            FillCommon(method, checker, calledField, instruction);
            //checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, instruction);
            //parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
    }
}