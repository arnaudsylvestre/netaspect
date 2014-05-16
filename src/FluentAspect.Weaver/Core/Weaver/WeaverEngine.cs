using System;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver
{

   public class WeaverEngine
   {
      private readonly IAssemblyPoolFactory assemblyPoolFactory;
      private readonly WeavingModelComputer weavingModelComputer;
       private readonly ErrorInfoComputer errorInfoComputer;

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
         var assemblyPool = assemblyPoolFactory.Create();

         var weavingModels_L = weavingModelComputer.ComputeWeavingModels(typesP_L, filter, assemblyPool, errorHandler);
         foreach (var weavingModel in weavingModels_L.weavingModels)
              weavingModel.Key.Weave(weavingModel.Value, errorHandler);

          assemblyPool.Save(errorHandler, newAssemblyNameProvider);
          return ConvertErrorReport(errorHandler, errorInfoComputer);
      }

       private ErrorReport ConvertErrorReport(ErrorHandler errorHandler, ErrorInfoComputer computer)
       {
           return new ErrorReport((from e in errorHandler.Errors
                                   select new ErrorReport.Error()
               {
                   Level = computer.ComputeLevel(e.Code),
                   Message                   = computer.ComputeMessage(e.Code, e.Parameters)
               }).ToList());
       }

       public interface IAssemblyPoolFactory
      {
         AssemblyPool Create();
      }
   }
}
