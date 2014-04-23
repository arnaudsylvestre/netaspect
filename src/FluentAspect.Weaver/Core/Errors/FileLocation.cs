namespace NetAspect.Weaver.Core.Errors
{
   public class FileLocation
   {
      public string FilePath { get; set; }
      public int Column { get; set; }
      public int Line { get; set; }
   }
}