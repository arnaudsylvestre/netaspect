using System;
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

    public static class CallWeavingGetPropertyInjectorFactory
    {
        public static IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction, PropertyDefinition property)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker, property, instruction);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, instruction, property);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction> parametersIlGenerator, Instruction instruction, PropertyDefinition property)
        {
            parametersIlGenerator.CreateIlGeneratorForCalledParameter();
            parametersIlGenerator.CreateIlGeneratorForCallerParameter();
            parametersIlGenerator.CreateIlGeneratorForCallerParameters();
            parametersIlGenerator.CreateIlGeneratorForCallerParametersName(method);
            parametersIlGenerator.CreateIlGeneratorForColumnNumber(instruction);
            parametersIlGenerator.CreateIlGeneratorForLineNumber(instruction);
            parametersIlGenerator.CreateIlGeneratorForFilename(instruction);
            parametersIlGenerator.CreateIlGeneratorForFilePath(instruction);
            parametersIlGenerator.CreateIlGeneratorForProperty(property, method.Module);
            //parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker, PropertyDefinition calledField, Instruction instruction)
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
            checker.CreateCheckerForProperty();
            //checker.CreateCheckerForParameterNameParameter(method);
            //checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect, Instruction instruction, PropertyDefinition property)
        {

            var checker = new ParametersChecker();
            FillCommon(method, checker, property, instruction);
            //checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            FillCommon(method, parametersIlGenerator, instruction, property);
            //parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }
    }
}