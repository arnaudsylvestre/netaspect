using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{

   public interface IWeavingModelFiller
   {
      void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel);
   }

   public class MultiWeavingModelFiller : IWeavingModelFiller
   {
      private IWeavingModelFiller[] weavingModelFillers;

      public MultiWeavingModelFiller(params IWeavingModelFiller[] weavingModelFillers_P)
      {
         weavingModelFillers = weavingModelFillers_P;
      }

      public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
      {
         foreach (var weavingModelFiller_L in weavingModelFillers)
         {
            weavingModelFiller_L.FillWeavingModel(method, aspect, weavingModel);
         }
      }
   }

   public class MethodAttributeWeavingModelFiller : IWeavingModelFiller
   {
      public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
      {
         var aspectType = method.Module.Import(aspect.Type);
         var isCompliant_L = method.CustomAttributes.Any(customAttribute_L => customAttribute_L.AttributeType == aspectType);
         if (!isCompliant_L)
            return;
         if (aspect.Before.Method != null)
         {
            weavingModel.Method.Befores.Add(new MethodWeavingBeforeMethodInjector(method, aspect.Before.Method));
         }
      }
   }



   public class InsideMethodInstructionWeavingModelFiller : IWeavingModelFiller
   {
      private Func<Instruction, NetAspectDefinition, bool> instructionCompliant;

      public InsideMethodInstructionWeavingModelFiller(Func<Instruction, NetAspectDefinition, bool> instructionCompliant_P)
      {
         instructionCompliant = instructionCompliant_P;
      }

      public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
      {
         if (method.Body == null)
            return;
         if (!method.Body.Instructions.Any(instruction_L => instructionCompliant(instruction_L, aspect)))
            return;
      }


   }

   public class WeaverCore2
   {
      public void Weave(Assembly assembly)
      {
         var typesP_L = assembly.GetTypes();
         IWeavingModelFiller weavingModelFiller = new MultiWeavingModelFiller(new MethodAttributeWeavingModelFiller());
         AssemblyDefinitionProvider assemblyDefinitionProvider_L = new AssemblyDefinitionProvider();
         
         List<NetAspectDefinition> aspects = FindAspects(typesP_L);
         HashSet<Assembly> assembliesToWeave = new HashSet<Assembly>(ComputeAssembliesToWeave(aspects));
         Dictionary<MethodDefinition, WeavingModel> weavingModels = new Dictionary<MethodDefinition, WeavingModel>();
         foreach (var assembly_L in assembliesToWeave)
         {
            foreach (var method in assemblyDefinitionProvider_L.GetAssemblyDefinition(assembly_L).GetAllMethods())
            {
               WeavingModel model = new WeavingModel();
               foreach (var aspect_L in aspects)
               {
                  weavingModelFiller.FillWeavingModel(method, aspect_L, model);
               }
               weavingModels.Add(method, model);
            }
         }

         AroundMethodWeaver aroundMethodWeaver_L = new AroundMethodWeaver();
         ErrorHandler errorHandler = new ErrorHandler();
         foreach (var weavingModel_L in weavingModels)
         {
            aroundMethodWeaver_L.Check(new Method(weavingModel_L.Key), weavingModel_L.Value.Method, errorHandler);
            aroundMethodWeaver_L.Weave(new Method(weavingModel_L.Key), weavingModel_L.Value.Method, errorHandler);
         }
      }

      private List<Assembly> ComputeAssembliesToWeave(List<NetAspectDefinition> aspects_P)
      {
         List<Assembly> assemblies_L = new List<Assembly>();

         foreach (var aspect_L in aspects_P)
         {
            assemblies_L.AddRange(aspect_L.AssembliesToWeave);
         }
         return assemblies_L;
      }

      private List<NetAspectDefinition> FindAspects(Type[] types_P)
      {
         return types_P.
                  Select(t => new NetAspectDefinition(t)).
                  Where(t => t.IsValid)
                  .ToList();
         
      }
   }

   public class New
   {
      public void Fill(WeavingConfiguration weavingConfiguration)
      {
         
      }
   }
}