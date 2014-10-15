using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Engine.Lifecycle
{
   public class TransientLifeCycleHandler : ILifeCycleHandler
   {
       public void CreateInterceptor(Type aspectType,
         MethodDefinition method_P,
         VariableDefinition interceptorVariable,
         List<Instruction> instructions)
      {
          instructions.AppendCreateNewObject(interceptorVariable, aspectType, method_P.Module, TODO);
      }
   }
}
