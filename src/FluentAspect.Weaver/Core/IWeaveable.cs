using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core
{
    public interface IWeaveable
    {
        void Weave();
        void Check(ErrorHandler error);
    }
}