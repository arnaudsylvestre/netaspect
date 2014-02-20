using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{


   public class WeavingModel
   {
      public WeavingModel()
      {
         Method = new MethodWeavingModel();
         Instructions = new Dictionary<Instruction, IInstructionIlInjector>();
      }

      public MethodWeavingModel Method { get; set; }
      public Dictionary<Instruction, IInstructionIlInjector> Instructions { get; set; }
   }

   public class MethodWeavingModel
   {
      public List<IIlInjector> Befores;
      public List<IIlInjector> Afters;
      public List<IIlInjector> OnExceptions;
      public List<IIlInjector> OnFinallys;
   }
}