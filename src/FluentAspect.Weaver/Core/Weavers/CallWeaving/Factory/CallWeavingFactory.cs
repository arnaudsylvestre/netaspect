using System.Collections.Generic;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Parameters;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory
{
    public class ParametersEngineFactory
    {
        public static ParametersEngine CreateForMethodCall(JoinPoint point, Dictionary<string, VariableDefinition> variablesForParameters)
        {
            var engine = new ParametersEngine();
            engine.AddLineNumber(point);
            engine.AddColumnNumber(point);
            engine.AddFilename(point);
            engine.AddFilepath(point);
            engine.AddCaller(point);
            engine.AddCallerParameters(point);
            engine.AddCalledParameters(point.Instruction.Operand as MethodReference, variablesForParameters);
            return engine;
        }
    }
}