using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory
{
    public static class ParametersEngineFactory
    {
       public static ParametersEngine CreateForBeforeMethodWeaving(MethodDefinition methodDefinition, VariableDefinition methodVariable, VariableDefinition parametersVariable, ErrorHandler errorHandler)
       {
          var engine = new ParametersEngine();
          FillCommon(engine, methodDefinition, methodVariable, parametersVariable, errorHandler);
          return engine;
       }

        private static void FillCommon(ParametersEngine engine, MethodDefinition methodDefinition, VariableDefinition methodVariable, VariableDefinition parametersVariable, ErrorHandler errorHandler)
        {
            engine.AddInstance(methodDefinition);
            engine.AddMethod(methodVariable);
            engine.AddParameters(parametersVariable);
            engine.AddParameterNames(methodDefinition, errorHandler);
        }

        public static ParametersEngine CreateForAfterMethodWeaving(MethodDefinition methodDefinition, VariableDefinition methodInfoVariable, VariableDefinition parametersVariable, VariableDefinition resultVariable, ErrorHandler errorHandler)
       {
           var engine = new ParametersEngine();
            FillCommon(engine, methodDefinition, methodInfoVariable, parametersVariable, errorHandler);
            engine.AddResult(methodDefinition, resultVariable);
          return engine;
       }
        
    }
}