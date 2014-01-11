using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model
{
    public class CallToWeave
    {
        public MethodDefinition MethodToWeave { get; set; }
        public IEnumerable<CallWeavingConfiguration> Interceptors { get; set; }
        public Instruction Instruction { get; set; }
    }
}