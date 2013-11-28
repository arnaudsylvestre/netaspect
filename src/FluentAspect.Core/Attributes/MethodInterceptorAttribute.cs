using System;

namespace FluentAspect.Core.Attributes
{
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
   public class MethodInterceptorAttribute : Attribute
   {
      public Type InterceptorType { get; private set; }

      public MethodInterceptorAttribute(Type interceptorType_P)
      {
         InterceptorType = interceptorType_P;
      }
   }
}