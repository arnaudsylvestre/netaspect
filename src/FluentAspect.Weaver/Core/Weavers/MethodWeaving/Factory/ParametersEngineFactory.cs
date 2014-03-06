using System;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory
{
    public static class ParametersEngineFactory
    {
        public static ParametersEngine CreateForBeforeMethodWeaving(MethodDefinition methodDefinition,
                                                                    Func<VariableDefinition> methodVariable,
                                                                    VariableDefinition parametersVariable,
                                                                    ErrorHandler errorHandler)
        {
            var engine = new ParametersEngine();
            FillCommon(engine, methodDefinition, methodVariable, parametersVariable, errorHandler);
            return engine;
        }

        public static ParametersEngine CreateForOnFinallyMethodWeaving(MethodDefinition methodDefinition,
                                                                       Func<VariableDefinition> methodVariable,
                                                                       VariableDefinition parametersVariable,
                                                                       ErrorHandler errorHandler)
        {
            var engine = new ParametersEngine();
            FillCommon(engine, methodDefinition, methodVariable, parametersVariable, errorHandler);
            return engine;
        }

        private static void FillCommon(ParametersEngine engine, MethodDefinition methodDefinition,
                                       Func<VariableDefinition> methodVariable, VariableDefinition parametersVariable,
                                       ErrorHandler errorHandler)
        {
            engine.AddInstance(methodDefinition);
            engine.AddMethod(methodVariable);
            engine.AddParameters(parametersVariable);
            engine.AddParameterNames(methodDefinition, errorHandler);
        }

        public static ParametersEngine CreateForAfterMethodWeaving(MethodDefinition methodDefinition,
                                                                   Func<VariableDefinition> methodInfoVariable,
                                                                   VariableDefinition parametersVariable,
                                                                   VariableDefinition resultVariable,
                                                                   ErrorHandler errorHandler)
        {
            var engine = new ParametersEngine();
            FillCommon(engine, methodDefinition, methodInfoVariable, parametersVariable, errorHandler);
            engine.AddResult(methodDefinition, resultVariable);
            return engine;
        }

        public static ParametersEngine CreateForOnExceptionMethodWeaving(MethodDefinition methodDefinition,
                                                                         Func<VariableDefinition> methodVariable,
                                                                         VariableDefinition parametersVariable,
                                                                         VariableDefinition exception,
                                                                         ErrorHandler errorHandler)
        {
            var engine = new ParametersEngine();
            FillCommon(engine, methodDefinition, methodVariable, parametersVariable, errorHandler);
            engine.AddException(exception);
            return engine;
        }

        public static ParametersEngine CreateForBeforeParameterWeaving(MethodDefinition methodDefinition,
                                                                       ParameterDefinition parameterDefinition_P,
                                                                       ErrorHandler errorHandler)
        {
            var engine = new ParametersEngine();
            engine.AddParameterValue(parameterDefinition_P);
            return engine;
        }
    }
}