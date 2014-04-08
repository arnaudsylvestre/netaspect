using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Checkers.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Checkers
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