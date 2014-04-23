using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver
{
    public interface IIlInjector<T>
    {
        void Check(ErrorHandler errorHandler, T availableInformations);
        void Inject(List<Instruction> instructions, T availableInformations);
    }
}