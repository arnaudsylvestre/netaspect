using System;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker;
using NetAspect.Weaver.Core.Assemblies;

namespace NetAspect.Weaver.Core.Weaver.Engine.AssemblyPoolFactories
{
   public class DefaultAssemblyPoolFactory : WeaverEngine.IAssemblyPoolFactory
   {
      private readonly IAssemblyChecker assemblyChecker;
      private Func<TypeDefinition, bool> typesToSave;

      public DefaultAssemblyPoolFactory(IAssemblyChecker assemblyChecker_P, Func<TypeDefinition, bool> typesToSave_P)
      {
         assemblyChecker = assemblyChecker_P;
         typesToSave = typesToSave_P;
      }

      public AssemblyPool Create()
      {
         return new AssemblyPool(assemblyChecker, typesToSave);
      }
   }
}