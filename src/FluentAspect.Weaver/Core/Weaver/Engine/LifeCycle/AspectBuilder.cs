using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.Engine.LifeCycle
{
   public class AspectBuilder
   {
      private readonly Dictionary<Model.Aspect.LifeCycle, ILifeCycleHandler> lifeCycles;

      public AspectBuilder(Dictionary<Model.Aspect.LifeCycle, ILifeCycleHandler> lifeCycles_P)
      {
         lifeCycles = lifeCycles_P;
      }

      public void CreateInterceptor(NetAspectDefinition aspect_P, MethodDefinition method_P, VariableDefinition interceptorVariable, List<Instruction> instructions)
      {
         lifeCycles[aspect_P.LifeCycle].CreateInterceptor(aspect_P, method_P, interceptorVariable, instructions);
      }
   }
}
