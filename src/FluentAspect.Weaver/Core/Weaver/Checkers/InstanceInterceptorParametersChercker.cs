using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    public class InstanceInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly MethodDefinition methodDefinition;

        public InstanceInterceptorParametersChercker(MethodDefinition methodDefinition)
        {
            this.methodDefinition = methodDefinition;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof (object).FullName, methodDefinition.DeclaringType.FullName);
            Ensure.NotStatic(parameter, errorListener, methodDefinition);
        }
    }
}