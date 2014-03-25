using System;

namespace FluentAspect.Sample.AOP
{
    public class CheckWithVoidInterceptorAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";
    }
}