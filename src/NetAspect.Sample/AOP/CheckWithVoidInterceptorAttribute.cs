using System;

namespace NetAspect.Sample.AOP
{
    public class CheckWithVoidInterceptorAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";
    }
}