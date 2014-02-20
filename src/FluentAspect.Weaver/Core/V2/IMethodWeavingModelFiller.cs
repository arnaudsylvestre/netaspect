using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
   public interface IMethodWeavingModelFiller
   {
      void Fill(MethodDefinition method, MethodWeavingModel model);
   }
}