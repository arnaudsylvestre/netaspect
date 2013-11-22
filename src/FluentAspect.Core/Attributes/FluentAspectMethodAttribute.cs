using System;
using FluentAspect.Core.Core;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Core.Attributes
{
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
   public class FluentAspectMethodAttribute : Attribute, IInterceptor
   {
      public virtual void Before(MethodCall call_P) { }
      public virtual void After(MethodCall call_P, MethodCallResult result_P) { }
      public virtual void OnException(MethodCall callP_P, Exception e) { }
   }
}