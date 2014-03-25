using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.Weaver.Checkers
{
    public class MethodInterceptorParametersChercker : IInterceptorParameterChecker
    {
        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof (MethodBase).FullName);
        }
    }
}