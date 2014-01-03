using System.Collections.Generic;
using System.Reflection;

namespace FluentAspect.Weaver.Core.Model
{
    public static class InterceptorExtensions
    {
        public static IEnumerable<ParameterInfo> GetParameters(this Interceptor interceptor)
        {
            if (interceptor.Method == null)
                return new ParameterInfo[0];
            return interceptor.Method.GetParameters();
        }
    }
}