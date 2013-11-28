using System;
using FluentAspect.Core.Attributes;

namespace FluentAspect.Sample.Attributes
{
   public class CheckBeforeAttribute : MethodInterceptorAttribute
   {
      public CheckBeforeAttribute()
         : base(typeof(CheckBeforeInterceptor))
      {
      }
   }
}