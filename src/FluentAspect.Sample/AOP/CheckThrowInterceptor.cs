using System;
using System.Reflection;

namespace FluentAspect.Sample.AOP
{
    public class CheckThrowInterceptorNetAspectAttribute : Attribute
   {
       public void Before(object instance, MethodInfo method, object[] parameters)
       {
           ((BeforeParameter)parameters[0]).Value = "Value set in before";
       }

       public void OnException(object instance, MethodInfo method, object[] parameters, Exception exception)
       {
           throw new NotSupportedException();
       }
   }
}