using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory
{
    public static class ParametersEngineFactory
    {
       public static ParametersEngine CreateForBeforeMethodWeaving(MethodDefinition methodDefinition, VariableDefinition methodVariable, VariableDefinition parametersVariable)
       {
          var engine = new ParametersEngine();
          FillCommon(engine, methodDefinition, methodVariable, parametersVariable);
          return engine;
       }

        private static void FillCommon(ParametersEngine engine, MethodDefinition methodDefinition, VariableDefinition methodVariable, VariableDefinition parametersVariable)
        {
            engine.AddInstance(methodDefinition);
            engine.AddMethod(methodVariable);
            engine.AddParameters(parametersVariable);
        }

        public static ParametersEngine CreateForAfterMethodWeaving(MethodDefinition methodDefinition, VariableDefinition methodInfoVariable, VariableDefinition parametersVariable, VariableDefinition resultVariable)
       {
           var engine = new ParametersEngine();
            FillCommon(engine, methodDefinition, methodInfoVariable, parametersVariable);
            engine.AddResult(methodDefinition, resultVariable);
          return engine;
       }
        
    }
}