using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class ResultInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private MethodDefinition methodDefinition;

        public ResultInterceptorParametersChercker(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
        }

        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.ResultOfType(parameter, errorListener, methodDefinition);
            //instructions.Add(Instruction.Create(info.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc, result))
        }
    }
}