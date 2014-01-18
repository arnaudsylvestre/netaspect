using System.Collections.Generic;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Parameters;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory
{
    public static class ParametersEngineFactory
    {
        public static ParametersEngine CreateForMethodCall(JoinPoint point, Dictionary<string, VariableDefinition> variablesForParameters)
        {
            var engine = new ParametersEngine();
            engine.AddPdbParameters(point);
            engine.AddCallerParams(point);
            engine.AddCalledParameters(point.Instruction.Operand as MethodReference, variablesForParameters);
            return engine;
        }
        public static ParametersEngine CreateForUpdateField(JoinPoint point)
        {
            var engine = new ParametersEngine();
            engine.AddPdbParameters(point);
            engine.AddCallerParams(point);
            engine.AddFieldValue(point);
            return engine;
        }
    }
}