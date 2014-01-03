using System;

namespace FluentAspect.Sample.AOP
{
    public class CheckWithVoidInterceptorAttribute : Attribute
    {
        private string NetAspectAttributeKind = "MethodWeaving";
    }
}