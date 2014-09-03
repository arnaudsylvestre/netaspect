using System;
using System.Reflection;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Helpers;

namespace NetAspect.Weaver.Core.Weaver
{
   public class WeaverEngine
   {
      private readonly IAssemblyPoolFactory assemblyPoolFactory;
      private readonly ErrorInfoComputer errorInfoComputer;
      private readonly WeavingModelComputer weavingModelComputer;

      public WeaverEngine(WeavingModelComputer weavingModelComputer, IAssemblyPoolFactory assemblyPoolFactory, ErrorInfoComputer errorInfoComputer)
      {
         this.weavingModelComputer = weavingModelComputer;
         this.assemblyPoolFactory = assemblyPoolFactory;
         this.errorInfoComputer = errorInfoComputer;
      }

      public ErrorReport Weave(string assemblyFilePath,
         Func<string, string> newAssemblyNameProvider)
      {
         return Weave(Assembly.LoadFrom(assemblyFilePath).GetTypes(), null, newAssemblyNameProvider);
      }

      public ErrorReport Weave(Type[] typesP_L, Type[] filter, Func<string, string> newAssemblyNameProvider)
      {
         var errorHandler = new ErrorHandler();
         AssemblyPool assemblyPool = assemblyPoolFactory.Create();

         MethodsWeavingModel weavingModels_L = weavingModelComputer.ComputeWeavingModels(typesP_L, filter, assemblyPool, errorHandler);
         foreach (var weavingModel in weavingModels_L.weavingModels)
            weavingModel.Key.Weave(weavingModel.Value, errorHandler);

         assemblyPool.Save(errorHandler, newAssemblyNameProvider);
         return ConvertErrorReport(errorHandler, errorInfoComputer);
      }

      private ErrorReport ConvertErrorReport(ErrorHandler errorHandler, ErrorInfoComputer computer)
      {
         return errorHandler.ConvertToErrorReport(computer);
      }

      public interface IAssemblyPoolFactory
      {
         AssemblyPool Create();
      }
   }
}
