using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class AspectBuilder
   {
      private Dictionary<LifeCycle, ILifeCycleHandler> lifeCycles;

      

      

      public AspectBuilder(Dictionary<LifeCycle, ILifeCycleHandler> lifeCycles_P)
      {
         lifeCycles = lifeCycles_P;
      }

      public void CreateInterceptor(NetAspectDefinition aspect_P, MethodDefinition method_P, VariableDefinition interceptorVariable, List<Instruction> instructions)
      {
         instructions.AppendCreateNewObject(interceptorVariable, aspect_P.Type, method_P.Module);
      }

   }
}