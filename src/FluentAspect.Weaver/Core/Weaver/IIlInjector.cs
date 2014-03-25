using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver
{
    public interface IIlInjector<T>
    {
        void Check(ErrorHandler errorHandler, T availableInformations);
        void Inject(List<Instruction> instructions, T availableInformations);
    }
}