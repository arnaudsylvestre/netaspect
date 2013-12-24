using System;
using System.Reflection;

namespace FluentAspect.Sample
{
   public class CheckWithParametersInterceptorNetAspectAttribute : Attribute
   {
       public void After(object[] parameters, ref string result)
       {
           result = parameters[0].ToString();
       }
   }
}