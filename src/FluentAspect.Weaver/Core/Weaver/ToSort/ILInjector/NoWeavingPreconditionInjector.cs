using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver
{
    public class NoWeavingPreconditionInjector : IWevingPreconditionInjector<VariablesForMethod>
    {
        public void Inject(List<Instruction> precondition, VariablesForMethod availableInformations, MethodDefinition method_P)
        {
        }
    }
}