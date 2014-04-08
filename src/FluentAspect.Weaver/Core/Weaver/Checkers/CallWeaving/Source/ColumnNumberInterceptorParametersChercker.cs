using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Source
{
    public class ColumnNumberInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly Instruction instruction;

        public ColumnNumberInterceptorParametersChercker(Instruction instruction)
        {
            this.instruction = instruction;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.SequencePoint(instruction, errorListener, parameter);
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType<int>(parameter, errorListener);
        }
    }
}