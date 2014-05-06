using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving
{
    public class ResultInterceptorParametersChercker
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