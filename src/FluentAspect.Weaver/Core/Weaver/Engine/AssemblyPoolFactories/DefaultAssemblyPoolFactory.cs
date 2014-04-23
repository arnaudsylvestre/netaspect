using NetAspect.Weaver.Apis.AssemblyChecker;
using NetAspect.Weaver.Core.Assemblies;

namespace NetAspect.Weaver.Core.Weaver.Engine.AssemblyPoolFactories
{
   public class DefaultAssemblyPoolFactory : WeaverEngine.IAssemblyPoolFactory
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