using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods
{
    public interface IMethodWeaver
    {
        void Init(Collection<VariableDefinition> variables, VariableDefinition result, ErrorHandler errorHandler);
        void InsertBefore(Collection<Instruction> method);
        void InsertAfter(Collection<Instruction> afterInstructions);
        void InsertOnException(Collection<Instruction> onExceptionInstructions);
        void InsertOnFinally(Collection<Instruction> onFinallyInstructions);
        void CheckBefore(ErrorHandler errorHandlerPP);
        void CheckAfter(ErrorHandler errorHandler);
        void InsertInitInstructions(Collection<Instruction> initInstructions);
        void CheckOnException(ErrorHandler errorHandler);
        void CheckOnFinally(ErrorHandler errorHandler);
    }
}