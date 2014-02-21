using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public class WeaverCore2
   {
      private WeavingModelComputer weavingModelComputer;
      AroundMethodWeaver aroundMethodWeaver_L = new AroundMethodWeaver();

      public WeaverCore2(WeavingModelComputer weavingModelComputer_P)
      {
         weavingModelComputer = weavingModelComputer_P;
      }

      public void Weave(Type[] typesP_L, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
      {
         var assemblyPool = new AssemblyPool();

          foreach (var weavingModel in ComputeWeavingModels(typesP_L, assemblyPool))
         {
            aroundMethodWeaver_L.Weave(new Method(weavingModel.Key), weavingModel.Value.Method, errorHandler);
         }

         assemblyPool.Save(errorHandler, newAssemblyNameProvider);
      }

       private Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(Type[] typesP_L, AssemblyPool assemblyPool)
       {
           var aspects = NetAspectDefinitionExtensions.FindAspects(typesP_L);
           var assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
           var weavingModels = weavingModelComputer.ComputeWeavingModels(assembliesToWeave, assemblyPool, aspects);
           return weavingModels;
       }

       public void Weave(string assemblyFilePath, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            Assembly mainAssembly = Assembly.LoadFrom(assemblyFilePath);
            var types = mainAssembly.GetTypes();
            Weave(types, errorHandler, newAssemblyNameProvider);
        }
   }
}