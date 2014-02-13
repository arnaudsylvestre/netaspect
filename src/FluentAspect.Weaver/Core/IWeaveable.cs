using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core
{
    public interface IWeaveable
    {
        void Check(ErrorHandler error);
        void Weave();
    }
}