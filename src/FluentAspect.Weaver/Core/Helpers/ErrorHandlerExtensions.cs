using System.Collections.Generic;
using System.Linq;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Weaver.Helpers
{
   public static class ErrorHandlerExtensions
   {
      public static ErrorReport ConvertToErrorReport(this ErrorHandler errorHandler,
         ErrorInfoComputer errorInfoComputer)
      {
         IEnumerable<ErrorReport.Error> errors = from e in errorHandler.Errors
            select new ErrorReport.Error
            {
               Level = errorInfoComputer.ComputeLevel(e.Code),
               Message = errorInfoComputer.ComputeMessage(e.Code, e.Parameters)
            };
         return new ErrorReport(errors);
      }
   }
}
