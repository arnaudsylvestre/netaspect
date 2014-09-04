using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver
{
    public interface IWevingPreconditionInjector
    {
        void Inject(List<Instruction> precondition, VariablesForMethod availableInformations, MethodInfo interceptorMethod_P, MethodDefinition method_P);
    }
}