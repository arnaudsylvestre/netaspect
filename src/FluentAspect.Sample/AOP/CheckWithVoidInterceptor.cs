using System;
using System.Reflection;

namespace FluentAspect.Sample
{
    public class CheckWithVoidInterceptorNetAspectAttribute : Attribute
   {
       public void Before(object instance, MethodInfo method, object[] parameters)
       {
       }

       public void After(object instance, MethodInfo method, object[] parameters, ref object result)
       {

       }

       public void OnException(object instance, MethodInfo method, object[] parameters, Exception exception)
       {
       }
   }
}