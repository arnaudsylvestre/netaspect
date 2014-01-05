using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Calls
{
    public class CallToWeave
    {
        public MethodDefinition MethodToWeave { get; set; }
        public List<CallWeavingConfiguration> Interceptors { get; set; }
        public Instruction Instruction { get; set; }
    }
}