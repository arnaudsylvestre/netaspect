using System;
using System.Reflection;

namespace FluentAspect.Sample
{
   public class ThrowerInterceptor
   {

       public void Before(object instance, MethodInfo method, object[] parameters)
       {
           if ((bool)parameters[0])
               throw new NotSupportedException();
       }

       public void After(object instance, MethodInfo method, object[] parameters, ref object result)
       {
       }

       public void OnException(object instance, MethodInfo method, object[] parameters, Exception exception)
       {
       }
   }
}