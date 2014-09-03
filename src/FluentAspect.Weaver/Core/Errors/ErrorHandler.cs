using System.Collections.Generic;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Errors
{
   public class ErrorHandler : IErrorListener
   {
      private readonly List<NetAspectError> errors = new List<NetAspectError>();

      public List<NetAspectError> Errors
      {
         get { return errors; }
      }

      public void OnError(ErrorCode code, FileLocation location, params object[] parameters)
      {
         errors.Add(new NetAspectError(code, parameters, location));
      }
   }
}
