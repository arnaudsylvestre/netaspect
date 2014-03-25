using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.Weaver.Checkers
{
    public class ParametersInterceptorParametersChercker : IInterceptorParameterChecker
    {
        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType<object[]>(parameter, errorListener);
        }
    }
}