using System;
using System.Linq;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
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
         var isCompliant_L = method.CustomAttributes.Any(customAttribute_L => customAttribute_L.AttributeType.FullName == aspectType.FullName);
         if (!isCompliant_L)
            return;
         if (aspect.Before.Method != null)
         {
            weavingModel.Method.Befores.Add(new MethodWeavingBeforeMethodInjector(method, aspect.Before.Method, aspect.Type));
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

   public class New
   {
      public void Fill(WeavingConfiguration weavingConfiguration)
      {
         
      }
   }
}