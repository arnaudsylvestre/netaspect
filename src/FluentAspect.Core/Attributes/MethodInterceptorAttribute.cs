using System;

namespace FluentAspect.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = true, Inherited = false)]
    public class MethodInterceptorAttribute : Attribute
    {
        public Type InterceptorType { get; private set; }

        public MethodInterceptorAttribute(Type interceptorType_P)
        {
            InterceptorType = interceptorType_P;
        }
    }
}