using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
   public static class NetAspectDefinitionExtensions
   {


      public static List<NetAspectDefinition> FindAspects(IEnumerable<Type> types_P)
      {
         return types_P.
            Select(t => new NetAspectDefinition(t)).
            Where(t => t.IsValid)
                       .ToList();

      }

      public static IEnumerable<Assembly> GetAssembliesToWeave(this IEnumerable<NetAspectDefinition> aspects_P, Assembly defaultAssembly)
      {
         HashSet<Assembly> assemblies_L = new HashSet<Assembly>();
         assemblies_L.Add(defaultAssembly);
         foreach (var aspect_L in aspects_P)
         {
            var assembliesToWeave = aspect_L.AssembliesToWeave;
            foreach (var assembly in assembliesToWeave)
            {
               assemblies_L.Add(assembly);

            }
         }
         return assemblies_L;
      }
   }

   public class WeavingModelComputer
   {
      IWeavingModelFiller weavingModelFiller = new MultiWeavingModelFiller(new MethodAttributeWeavingModelFiller());

      public Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(IEnumerable<Assembly> assembliesToWeave, AssemblyDefinitionProvider assemblyDefinitionProvider, IEnumerable<NetAspectDefinition> aspects)
      {
         var weavingModels = new Dictionary<MethodDefinition, WeavingModel>();
         foreach (var assembly_L in assembliesToWeave)
         {
            foreach (var method in assemblyDefinitionProvider.GetAssemblyDefinition(assembly_L).GetAllMethods())
            {
               WeavingModel model = new WeavingModel();
               foreach (var aspect_L in aspects)
               {
                  weavingModelFiller.FillWeavingModel(method, aspect_L, model);
               }
               if (!model.IsEmpty)
                  weavingModels.Add(method, model);
            }
         }
         return weavingModels;
      }
   }

   public class AssemblyPool
   {
      Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>();

      public Dictionary<Assembly, AssemblyDefinition> Asms
      {
         get { return asms; }
      }

      public void Add(Assembly assembly)
      {
         asms.Add(assembly, AssemblyDefinition.ReadAssembly(assembly.GetAssemblyPath(), new ReaderParameters(ReadingMode.Immediate)
            {
               ReadSymbols = true
            }));
      }

      public AssemblyDefinition GetAssemblyDefinition(Assembly assembly)
      {
         return asms[assembly];
      }

      public void Save(ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider, AssemblyDefinitionProvider assemblyDefinitionProvider_L)
      {
         foreach (var def in assemblyDefinitionProvider_L.Asms)
         {
            WeaverCore.WeaveOneAssembly(def.Key.GetAssemblyPath(), def.Value, errorHandler, newAssemblyNameProvider);
         }
         foreach (var def in assemblyDefinitionProvider_L.Asms)
         {
            WeaverCore.CheckAssembly(def.Key.GetAssemblyPath(), errorHandler);
         }
      }
   }


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
         
         var assemblyDefinitionProvider_L = new AssemblyDefinitionProvider();

         var aspects = NetAspectDefinitionExtensions.FindAspects(typesP_L);
         var assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
         var weavingModels = weavingModelComputer.ComputeWeavingModels(assembliesToWeave, assemblyDefinitionProvider_L, aspects);

         foreach (var weavingModel_L in weavingModels)
         {
            aroundMethodWeaver_L.Weave(new Method(weavingModel_L.Key), weavingModel_L.Value.Method, errorHandler);
         }

         GenerateAssemblies(errorHandler, newAssemblyNameProvider, assemblyDefinitionProvider_L);
      }

      
   }
}