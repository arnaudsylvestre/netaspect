using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core
{
   public interface IWeaveable
   {
      void Check(ErrorHandler errorHandler);
      void Weave(ErrorHandler errorP_P);
   }
}