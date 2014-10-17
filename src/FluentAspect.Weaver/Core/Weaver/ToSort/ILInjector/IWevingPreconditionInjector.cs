using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver
{
    public interface IWevingPreconditionInjector<in T>
        where T : VariablesForMethod
    {
        void Inject(List<Instruction> precondition, T availableInformations, MethodDefinition method_P);
    }
}