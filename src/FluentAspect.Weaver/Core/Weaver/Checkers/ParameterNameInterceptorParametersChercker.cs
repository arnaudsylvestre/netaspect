using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weaver.Checkers
{
    public class ParameterNameInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly ParameterDefinition _parameterName;

        public ParameterNameInterceptorParametersChercker(ParameterDefinition parameterName)
        {
            _parameterName = parameterName;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.OfType(parameter, errorListener, _parameterName);
            Ensure.NotOut(parameter, errorListener);
        }
    }
}