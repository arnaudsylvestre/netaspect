using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public interface ICallWeavingProvider
    {
        void AddBefore(List<Instruction> beforeInstructions);
        void AddAfter(List<Instruction> beforeInstructions);
        void Check(ErrorHandler error);
    }
}