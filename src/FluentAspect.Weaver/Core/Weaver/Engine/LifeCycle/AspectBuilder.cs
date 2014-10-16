using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.Engine.Lifecycle
{
   public class AspectBuilder
   {
      private readonly Dictionary<LifeCycle, ILifeCycleHandler> lifeCycles;

      public AspectBuilder(Dictionary<LifeCycle, ILifeCycleHandler> lifeCycles_P)
      {
         lifeCycles = lifeCycles_P;
      }

      public void CreateInterceptor(Type aspectType, LifeCycle lifeCycle, MethodDefinition method_P, VariableDefinition interceptorVariable, List<Instruction> instructions, CustomAttribute customAttribute)
      {
          lifeCycles[lifeCycle].CreateInterceptor(aspectType, method_P, interceptorVariable, instructions, customAttribute);
      }
   }
}
