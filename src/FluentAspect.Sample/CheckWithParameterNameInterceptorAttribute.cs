using System;

namespace NetAspect.Sample
{
    public class CheckWithParameterNameInterceptorAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(int first, ref int second)
        {
            second = first + 1;
        }
    }
}