using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public interface IIlInjector<T>
    {
        void Check(ErrorHandler errorHandler);
        void Inject(List<Instruction> instructions, T availableInformations);
    }
}