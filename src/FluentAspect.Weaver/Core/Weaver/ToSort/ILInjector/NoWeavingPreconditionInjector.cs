using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
    public class NoWeavingPreconditionInjector : IWevingPreconditionInjector<VariablesForMethod>
    {
        public void Inject(List<Mono.Cecil.Cil.Instruction> precondition, VariablesForMethod availableInformations, MethodDefinition method_P)
        {
        }
    }
}