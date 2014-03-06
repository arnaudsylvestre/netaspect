using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Methods
{
    public class MethodCallToWeave
    {
        public JoinPoint JoinPoint { get; set; }
        public IEnumerable<CallWeavingConfiguration> Interceptors { get; set; }

        public MethodReference CalledMethod
        {
            get { return JoinPoint.InstructionStart.Operand as MethodReference; }
        }
    }
}