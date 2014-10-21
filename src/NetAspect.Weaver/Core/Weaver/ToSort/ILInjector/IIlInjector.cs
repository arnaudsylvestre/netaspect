using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
   public interface IIlInjector<T>
   {
       void Check(ErrorHandler errorHandler, T availableInformations, Variable aspectInstance);
       void Inject(List<Mono.Cecil.Cil.Instruction> instructions, T availableInformations, Variable aspectInstance);
   }
}
