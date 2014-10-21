using System;
using System.Collections.Generic;
using System.Linq;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Model.Errors
{
   public class ErrorReport
   {
      public ErrorReport(IEnumerable<Error> errors)
      {
         Errors = new List<Error>(errors);
      }

      public List<Error> Errors { get; private set; }

      public IEnumerable<string> Warnings
      {
         get { return from e in Errors where e.Level == ErrorLevel.Warning select e.Message; }
      }

      public IEnumerable<string> ErrorsAndFailures
      {
         get { return from e in Errors where e.Level != ErrorLevel.Warning select e.Message; }
      }

      [Serializable]
      public class Error
      {
         public ErrorLevel Level { get; set; }
         public string Message { get; set; }
      }
   }
}
