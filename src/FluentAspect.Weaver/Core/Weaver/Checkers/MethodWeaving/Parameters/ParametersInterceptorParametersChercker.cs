using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Parameters
{
    public class ParametersInterceptorParametersChercker
    {
        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType<object[]>(parameter, errorListener);
        }
    }
}