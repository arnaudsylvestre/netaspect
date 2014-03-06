using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class ParameterNameInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly ParameterDefinition _parameterName;

        public ParameterNameInterceptorParametersChercker(ParameterDefinition parameterName)
        {
            _parameterName = parameterName;
        }

        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.OfType(parameter, errorListener, _parameterName);
        }
    }
}