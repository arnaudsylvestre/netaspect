using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Data;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public interface IIlInjector
   {
      void Check(ErrorHandler errorHandler);
      void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations);
   }
}
