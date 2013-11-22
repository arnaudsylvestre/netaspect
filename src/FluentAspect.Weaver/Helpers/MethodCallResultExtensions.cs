namespace FluentAspect.Core.Expressions
{
   public static class MethodCallResultExtensions
   {
      public static MethodCallResult Clone(this MethodCallResult result_P)
      {
         return new MethodCallResult {Result = result_P};
      }
   }
}