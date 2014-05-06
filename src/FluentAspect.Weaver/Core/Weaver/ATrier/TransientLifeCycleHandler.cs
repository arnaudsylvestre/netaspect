using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class TransientLifeCycleHandler : ILifeCycleHandler
   {
      public void CreateInterceptor(AspectBuilder aspect, VariableDefinition interceptor)
      {
         //aspect.Instructions.AppendCreateNewObject(interceptor, aspect.Aspect.Type, aspect.Method.Module);
      }
   }
}