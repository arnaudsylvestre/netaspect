using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
   public interface IIlInjector<T>
   {
       void Check(ErrorHandler errorHandler, T availableInformations);
      void Inject(List<Mono.Cecil.Cil.Instruction> instructions, T availableInformations);
   }
}
