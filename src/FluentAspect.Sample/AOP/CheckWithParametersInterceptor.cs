using System;

namespace FluentAspect.Sample.AOP
{
   public class CheckWithParametersInterceptorNetAspectAttribute : Attribute
   {
       public void After(object[] parameters, ref string result)
       {
           result = parameters[0].ToString();
       }
   }
}