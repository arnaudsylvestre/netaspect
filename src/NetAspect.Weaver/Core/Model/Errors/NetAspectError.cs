namespace NetAspect.Weaver.Core.Model.Errors
{
   public class NetAspectError
   {
      public NetAspectError(ErrorCode code, object[] parameters, FileLocation location)
      {
         Code = code;
         Parameters = parameters;
         Location = location;
      }

      public ErrorCode Code { get; set; }
      public object[] Parameters { get; set; }
      public FileLocation Location { get; private set; }
   }
}
