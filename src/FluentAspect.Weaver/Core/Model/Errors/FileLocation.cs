namespace NetAspect.Weaver.Core.Model.Errors
{
   public class FileLocation
   {
       public static readonly FileLocation None = new FileLocation();
       public string FilePath { get; set; }
      public int Column { get; set; }
      public int Line { get; set; }
   }
}