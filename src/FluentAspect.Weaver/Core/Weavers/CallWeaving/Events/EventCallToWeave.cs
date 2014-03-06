using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Events
{
    public class EventCallToWeave
    {
        public JoinPoint JoinPoint { get; set; }
        public IEnumerable<CallWeavingConfiguration> Interceptors { get; set; }
        public MethodReference Call
        {
            get { return JoinPoint.InstructionEnd.Operand as MethodReference; }
        }
        public FieldReference Field
        {
            get { return JoinPoint.InstructionStart.Operand as FieldReference; }
        }
    }
}