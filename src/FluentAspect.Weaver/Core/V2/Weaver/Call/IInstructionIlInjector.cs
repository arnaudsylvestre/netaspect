using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public interface IInstructionIlInjector
    {
        void Check(ErrorHandler errorHandler, IlInstructionInjectorAvailableVariables availableInformations);
        void Inject(List<Instruction> instructions, IlInstructionInjectorAvailableVariables availableInformations);
    }

    public interface IlInstructionInjectorAvailableVariables
    {
    }
}