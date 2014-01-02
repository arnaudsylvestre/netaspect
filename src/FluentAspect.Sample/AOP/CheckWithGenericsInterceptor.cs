using System;

namespace FluentAspect.Sample.AOP
{
    class CheckWithGenericsInterceptorNetAspectAttribute : Attribute
    {

        public void After(ref string result)
        {
            result = result + "Weaved";
        }
    }
}
