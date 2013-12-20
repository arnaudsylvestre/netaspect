using System;
using System.Reflection;

namespace FluentAspect.Sample
{
   class CheckWithGenericsInterceptor
    {
        public void Before(object instance, MethodInfo method, object[] parameters)
        {
        }

        public void After(object instance, MethodInfo method, object[] parameters, ref object result)
        {
            result = result + "Weaved";
        }

        public void OnException(object instance, MethodInfo method, object[] parameters, Exception exception)
        {
        }
    }
}
