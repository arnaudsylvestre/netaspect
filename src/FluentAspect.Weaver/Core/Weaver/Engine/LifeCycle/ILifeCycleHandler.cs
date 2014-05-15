using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public interface ILifeCycleHandler
   {
       void CreateInterceptor(NetAspectDefinition aspect_P, MethodDefinition method_P,
                               VariableDefinition interceptorVariable, List<Instruction> instructions);
   }
}