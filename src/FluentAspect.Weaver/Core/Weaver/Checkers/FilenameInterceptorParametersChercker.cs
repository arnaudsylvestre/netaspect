using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    public class FilenameInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly Instruction instruction;

        public FilenameInterceptorParametersChercker(Instruction instruction)
        {
            this.instruction = instruction;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.SequencePoint(instruction, errorListener, parameter);
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType<string>(parameter, errorListener);
        }
    }
}