using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;

namespace FluentAspect.Weaver.Core.V2.Weaver.Checkers
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