using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
    public interface IWevingPreconditionInjector<in T>
        where T : VariablesForMethod
    {
        void Inject(List<Mono.Cecil.Cil.Instruction> precondition, T availableInformations, MethodDefinition method_P);
    }
}