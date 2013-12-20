using System;
using System.Reflection;

namespace FluentAspect.Sample
{
   public class CheckWithParametersInterceptor
   {
       public void Before(object instance, MethodInfo method, object[] parameters)
       {
       }

       public void After(object instance, MethodInfo method, object[] parameters, ref object result)
       {
           result = parameters[0];
       }

       public void OnException(object instance, MethodInfo method, object[] parameters, Exception exception)
       {
       }
   }
}