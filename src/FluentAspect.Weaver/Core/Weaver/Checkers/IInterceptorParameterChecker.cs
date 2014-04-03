using System.Reflection;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    public interface IInterceptorParameterChecker
    {
        void Check(ParameterInfo parameter, ErrorHandler errorListener);
    }
}