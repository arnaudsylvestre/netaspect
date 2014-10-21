using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
   public class NoIIlInjector<T> : IIlInjector<T>
   {
       public void Check(ErrorHandler errorHandler, T info, Variable aspectInstance)
      {
      }

       public void Inject(List<Mono.Cecil.Cil.Instruction> instructions, T availableInformations, Variable aspectInstance)
      {
      }
   }
}
