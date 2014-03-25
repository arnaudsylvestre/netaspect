using System;

namespace FluentAspect.Sample.AOP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckMultiAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";

        public void Before(ref int i)
        {
            i++;
        }
    }
}