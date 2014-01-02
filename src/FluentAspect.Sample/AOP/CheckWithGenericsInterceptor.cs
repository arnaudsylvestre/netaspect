using System;

namespace FluentAspect.Sample.AOP
{
    class CheckWithGenericsInterceptorAttribute : Attribute
    {
       string NetAspectAttributeKind = "MethodWeaving";
        public void After(ref string result)
        {
            result = result + "Weaved";
        }
    }
}
