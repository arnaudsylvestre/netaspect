using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Apis.AssemblyChecker
{
    public interface IAssemblyChecker
    {
        void Check(string assemblyFile, ErrorHandler errorHandler);
    }
}