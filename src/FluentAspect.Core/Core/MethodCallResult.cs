namespace FluentAspect.Core.Expressions
{
   public class MethodCallResult
   {
       public MethodCallResult(object result)
       {
           Result = result;
       }

       public object Result { get; set; }
   }
}