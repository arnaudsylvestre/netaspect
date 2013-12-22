using System;
using System.Reflection;

namespace FluentAspect.Sample
{
    public class CheckMultiInterceptor
    {
        public void Before(ref int i)
        {
            i++;
        }
    }
}