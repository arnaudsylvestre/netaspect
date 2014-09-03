using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Engine.LifeCycle
{
   public class TransientLifeCycleHandler : ILifeCycleHandler
   {
      public void CreateInterceptor(NetAspectDefinition aspect_P,
         MethodDefinition method_P,
         VariableDefinition interceptorVariable,
         List<Instruction> instructions)
      {
         instructions.AppendCreateNewObject(interceptorVariable, aspect_P.Type, method_P.Module);
      }
   }
}
