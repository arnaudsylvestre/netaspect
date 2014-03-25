using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.V2.Weaver.Checkers
{
    public interface IInterceptorParameterChecker
    {
        void Check(ParameterInfo parameter, ErrorHandler errorListener);
    }
}