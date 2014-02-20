using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
   public class MultiMethodWeavingModelFiller : IMethodWeavingModelFiller
   {
      private List<IMethodWeavingModelFiller> fillers;

      public MultiMethodWeavingModelFiller(List<IMethodWeavingModelFiller> fillers_P)
      {
         fillers = fillers_P;
      }

      public void Fill(MethodDefinition method, MethodWeavingModel model)
      {
         foreach (var filler_L in fillers)
         {
            filler_L.Fill(method, model);
         }
      }
   }
}