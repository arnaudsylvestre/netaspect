using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public interface ILifeCycleHandler
   {
      void CreateInterceptor(AspectBuilder aspect_P, VariableDefinition interceptor);
   }
}