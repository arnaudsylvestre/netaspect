#region

using System;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Engine;

#endregion

namespace NetAspect.Weaver.Core.Weaver
{
   public class WeaverEngine
   {
      private readonly IAssemblyPoolFactory _assemblyPoolFactory;
      private readonly WeavingModelComputer2 _weavingModelComputer;

      public WeaverEngine(WeavingModelComputer2 weavingModelComputer_P, IAssemblyPoolFactory assemblyPoolFactory_P)
      {
         _weavingModelComputer = weavingModelComputer_P;
         _assemblyPoolFactory = assemblyPoolFactory_P;
      }

      public void Weave(string assemblyFilePath,
                        ErrorHandler errorHandler,
                        Func<string, string> newAssemblyNameProvider)
      {
         Weave(Assembly.LoadFrom(assemblyFilePath).GetTypes(), null, errorHandler, newAssemblyNameProvider);
      }

      public void Weave(Type[] typesP_L, Type[] filter, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
      {
         var assemblyPool = _assemblyPoolFactory.Create();

         var weavingModels_L = _weavingModelComputer.ComputeWeavingModels(typesP_L, filter, assemblyPool, errorHandler);
         foreach (var weavingModel in weavingModels_L)
         {
            weavingModel.Key.Weave(weavingModel.Value, errorHandler);
         }

         assemblyPool.Save(errorHandler, newAssemblyNameProvider);
      }
   }
}
