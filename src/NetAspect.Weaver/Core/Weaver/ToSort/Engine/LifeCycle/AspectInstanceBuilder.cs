using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine.LifeCycle
{
   public class AspectInstanceBuilder
   {
      private readonly Dictionary<Model.Aspect.LifeCycle, ILifeCycleHandler> lifeCycles;

      public AspectInstanceBuilder(Dictionary<Model.Aspect.LifeCycle, ILifeCycleHandler> lifeCycles_P)
      {
         lifeCycles = lifeCycles_P;
      }

      public void CreateAspectInstance(Type aspectType, Model.Aspect.LifeCycle lifeCycle, MethodDefinition method_P, VariableDefinition interceptorVariable, List<Mono.Cecil.Cil.Instruction> instructions, CustomAttribute customAttribute)
      {
          lifeCycles[lifeCycle].CreateInterceptor(aspectType, method_P, interceptorVariable, instructions, customAttribute);
      }
   }
}
