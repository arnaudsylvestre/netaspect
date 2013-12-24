using System;
using System.Reflection;

namespace FluentAspect.Sample
{
    class CheckWithGenericsInterceptorNetAspectAttribute : Attribute
    {

        public void After(ref string result)
        {
            result = result + "Weaved";
        }
    }
}
