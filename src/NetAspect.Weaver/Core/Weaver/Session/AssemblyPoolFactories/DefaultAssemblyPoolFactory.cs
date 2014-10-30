using System;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker;
using NetAspect.Weaver.Core.Assemblies;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine.AssemblyPoolFactories
{
   public class DefaultAssemblyPoolFactory : WeaverEngine.IAssemblyPoolFactory
   {
      private readonly IAssemblyChecker assemblyChecker;
      private readonly Func<TypeDefinition, bool> typesToSave;
       private string assemblyPath;

       public DefaultAssemblyPoolFactory(IAssemblyChecker assemblyChecker_P, Func<TypeDefinition, bool> typesToSave_P, string assemblyPath)
      {
         assemblyChecker = assemblyChecker_P;
         typesToSave = typesToSave_P;
           this.assemblyPath = assemblyPath;
      }

      public AssemblyPool Create()
      {
         return new AssemblyPool(assemblyChecker, typesToSave, assemblyPath);
      }
   }
}
