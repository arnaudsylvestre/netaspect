using System;

namespace FluentAspect.Sample.AOP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckMultiAttribute : Attribute
    {
       string NetAspectAttributeKind = "MethodWeaving";

        public void Before(ref int i)
        {
            i++;
        }
    }
}