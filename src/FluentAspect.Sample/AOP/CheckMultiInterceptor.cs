using System;
using System.Reflection;

namespace FluentAspect.Sample
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