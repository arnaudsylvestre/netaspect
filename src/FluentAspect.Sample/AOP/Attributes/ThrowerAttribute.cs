using System;
using FluentAspect.Core.Attributes;

namespace FluentAspect.Sample.Attributes
{
    public class ThrowerAttribute : MethodInterceptorAttribute
    {
        public ThrowerAttribute()
            : base(typeof(ThrowerInterceptor))
        {
        }
    }
}