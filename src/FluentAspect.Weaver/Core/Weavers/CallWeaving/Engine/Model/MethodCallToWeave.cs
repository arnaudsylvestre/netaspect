using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model
{
    public class MethodCallToWeave
    {
        public JoinPoint JoinPoint { get; set; }
        public IEnumerable<CallWeavingConfiguration> Interceptors { get; set; }
        public MethodReference CalledMethod
        {
            get { return JoinPoint.Instruction.Operand as MethodReference; }
        }
    }
}