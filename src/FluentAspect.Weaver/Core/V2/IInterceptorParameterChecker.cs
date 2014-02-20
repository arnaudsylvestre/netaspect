using System.Reflection;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public interface IInterceptorParameterChecker
    {
        void Check(ParameterInfo parameter, IErrorListener errorListener);
    }
}