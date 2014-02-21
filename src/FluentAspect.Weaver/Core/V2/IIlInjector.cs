using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
   public interface IIlInjector
   {
      void Check(ErrorHandler errorHandler, IlInjectorAvailableVariables availableInformations);
      void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations);
   }
}