using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Checkers.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    public class FieldInterceptorParametersChercker : IInterceptorParameterChecker
    {
        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType<FieldInfo>(parameter, errorListener);
        }
    }
}