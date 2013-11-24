using System.Collections.Generic;
using FluentAspect.Core.Expressions;

namespace FluentAspect.Core.Core
{
   public static class MethodCallExtensions
   {
      public static MethodCall Clone(this MethodCall call_P)
      {
         return new MethodCall(call_P.This, call_P.Method, new List<object>(call_P.Parameters).ToArray());
      }
   }
}