using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.Engine.Lifecycle
{
   public interface ILifeCycleHandler
   {
      void CreateInterceptor(Type aspectType,
         MethodDefinition method_P,
         VariableDefinition interceptorVariable,
         List<Instruction> instructions);
   }
}
