using System;

namespace FluentAspect.Sample.AOP
{
    internal class CheckWithGenericsInterceptorAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref string result)
        {
            result = result + "Weaved";
        }
    }
}