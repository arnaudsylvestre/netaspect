using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core
{
   public interface IWeaveable
   {
       void Weave(ErrorHandler errorP_P);
   }
}