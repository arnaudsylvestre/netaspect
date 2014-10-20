using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
   public class NoIIlInjector<T> : IIlInjector<T>
   {
       public void Check(ErrorHandler errorHandler, T info)
      {
      }

      public void Inject(List<Mono.Cecil.Cil.Instruction> instructions, T availableInformations)
      {
      }
   }
}
