using System;

namespace FluentAspect.Sample.AOP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckMultiAttribute : Attribute
    {
        private string NetAspectAttributeKind = "MethodWeaving";

        public void Before(ref int i)
        {
            i++;
        }
    }
}