using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public interface IInterceptorParameterChecker
    {
        void Check(ParameterInfo parameter, ErrorHandler errorListener);
    }
}