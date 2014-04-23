using NetAspect.Weaver.Apis.AssemblyChecker;
using NetAspect.Weaver.Core.Assemblies;

namespace NetAspect.Weaver.Core.Weaver
{
   public class DefaultAssemblyPoolFactory : IAssemblyPoolFactory
   {
      private readonly IAssemblyChecker assemblyChecker;

      public DefaultAssemblyPoolFactory(IAssemblyChecker assemblyChecker_P)
      {
         assemblyChecker = assemblyChecker_P;
      }

      public AssemblyPool Create()
      {
         return new AssemblyPool(assemblyChecker);
      }
   }
}