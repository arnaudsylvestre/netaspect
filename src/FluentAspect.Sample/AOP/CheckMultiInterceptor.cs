using System;

namespace FluentAspect.Sample.AOP
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CheckMultiNetAspectAttribute : Attribute
    {
        public void Before(ref int i)
        {
            i++;
        }
    }
}