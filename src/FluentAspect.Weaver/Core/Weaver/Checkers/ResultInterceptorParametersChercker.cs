using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weaver.Checkers
{
    public class ResultInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly MethodDefinition methodDefinition;

        public ResultInterceptorParametersChercker(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.ResultOfType(parameter, errorListener, methodDefinition);
            //instructions.Add(Instruction.Create(info.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, result))
        }
    }
}