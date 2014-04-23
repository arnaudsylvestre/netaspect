using System;
using System.Reflection;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver
{
   public class WeaverEngine
   {
      private readonly IAssemblyPoolFactory assemblyPoolFactory;
      private readonly WeavingModelComputer2 weavingModelComputer;

      public WeaverEngine(WeavingModelComputer2 weavingModelComputer, IAssemblyPoolFactory assemblyPoolFactory)
      {
         this.weavingModelComputer = weavingModelComputer;
         this.assemblyPoolFactory = assemblyPoolFactory;
      }

      public void Weave(string assemblyFilePath,
                        ErrorHandler errorHandler,
                        Func<string, string> newAssemblyNameProvider)
      {
         Weave(Assembly.LoadFrom(assemblyFilePath).GetTypes(), null, errorHandler, newAssemblyNameProvider);
      }

      public void Weave(Type[] typesP_L, Type[] filter, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
      {
         var assemblyPool = assemblyPoolFactory.Create();

         var weavingModels_L = weavingModelComputer.ComputeWeavingModels(typesP_L, filter, assemblyPool, errorHandler);
         foreach (var weavingModel in weavingModels_L)
         {
            weavingModel.Key.Weave(weavingModel.Value, errorHandler);
         }

         assemblyPool.Save(errorHandler, newAssemblyNameProvider);
      }

      public interface IAssemblyPoolFactory
      {
         AssemblyPool Create();
      }
   }
}
