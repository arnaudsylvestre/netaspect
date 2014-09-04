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
       private MethodWeaver methodWeaver;

      public WeaverEngine(WeavingModelComputer weavingModelComputer, IAssemblyPoolFactory assemblyPoolFactory, ErrorInfoComputer errorInfoComputer, MethodWeaver methodWeaver)
      {
         this.weavingModelComputer = weavingModelComputer;
         this.assemblyPoolFactory = assemblyPoolFactory;
         this.errorInfoComputer = errorInfoComputer;
          this.methodWeaver = methodWeaver;
      }

      public ErrorReport Weave(string assemblyFilePath, Func<string, string> newAssemblyNameProvider)
      {
         return Weave(Assembly.LoadFrom(assemblyFilePath).GetTypes(), null, newAssemblyNameProvider);
      }

      public ErrorReport Weave(Type[] types, Type[] filter, Func<string, string> newAssemblyNameProvider)
      {
         var errorHandler = new ErrorHandler();
         AssemblyPool assemblyPool = assemblyPoolFactory.Create();

         MethodsWeavingModel weavingModels_L = weavingModelComputer.ComputeWeavingModels(types, filter, assemblyPool, errorHandler);
         foreach (var weavingModel in weavingModels_L.weavingModels)
             methodWeaver.Weave(weavingModel.Key, weavingModel.Value, errorHandler);

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
