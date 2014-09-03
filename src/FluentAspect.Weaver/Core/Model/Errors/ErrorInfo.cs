namespace NetAspect.Weaver.Core.Model.Errors
{
   public class ErrorInfo
   {
      public ErrorInfo(ErrorLevel level, string message)
      {
         Level = level;
         Message = message;
      }

      public ErrorInfo(string message)
         : this(ErrorLevel.Error, message)
      {
      }

      public ErrorLevel Level { get; set; }
      public string Message { get; set; }
   }
}
