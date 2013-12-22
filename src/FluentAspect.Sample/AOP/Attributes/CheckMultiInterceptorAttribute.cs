using System;
using FluentAspect.Core.Attributes;

namespace FluentAspect.Sample.Attributes
{
    public class CheckMultiInterceptorAttribute : MethodInterceptorAttribute
    {
        public CheckMultiInterceptorAttribute()
            : base(typeof(CheckMultiInterceptor))
        {
        }
    }
}