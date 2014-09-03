using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Apis.AssemblyChecker
{
   public interface IAssemblyChecker
   {
      void Check(string assemblyFile, ErrorHandler errorHandler);
   }
}
