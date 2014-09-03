using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ATrier;
using NetAspect.Weaver.Core.Weaver.Data;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public class NoIIlInjector : IIlInjector
   {
      public void Check(ErrorHandler errorHandler)
      {
      }

      public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
      {
      }
   }
}
