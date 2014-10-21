using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine.LifeCycle
{
   public class TransientLifeCycleHandler : ILifeCycleHandler
   {
       public void CreateInterceptor(Type aspectType,
         MethodDefinition method_P,
         VariableDefinition interceptorVariable,
         List<Mono.Cecil.Cil.Instruction> instructions, CustomAttribute attibute)
      {
          instructions.AppendCreateNewObject(interceptorVariable, aspectType, method_P.Module, attibute);
      }
   }
}
