using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
{
    public class MethodInterceptorParametersChercker : IInterceptorParameterChecker
    {

        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType(parameter, errorListener, typeof(MethodInfo).FullName);
        }
    }
}