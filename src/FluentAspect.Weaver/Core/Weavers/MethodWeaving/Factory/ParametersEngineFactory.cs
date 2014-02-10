using System.Collections.Generic;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Parameters;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory
{
    public static class ParametersEngineFactory
    {
       public static ParametersEngine CreateForBeforeMethodWeaving(MethodDefinition methodDefinition)
       {
          var engine = new ParametersEngine();
          engine.AddInstance(methodDefinition);
          return engine;
       }
       public static ParametersEngine CreateForAfterMethodWeaving(MethodDefinition methodDefinition)
       {
          var engine = new ParametersEngine();
          engine.AddInstance(methodDefinition);
          return engine;
       }
        
    }
}