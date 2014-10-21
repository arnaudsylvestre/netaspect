using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Tests.Helpers
{
   public static class ErrorHandlerExtensions
   {
      public static void Dump(this ErrorReport errorHandler, StringBuilder builder)
      {
         Dump("Warnings", from e in errorHandler.Errors where e.Level == ErrorLevel.Warning select e.Message, builder);
         Dump("Errors", from e in errorHandler.Errors where e.Level == ErrorLevel.Error select e.Message, builder);
         Dump("Failures", from e in errorHandler.Errors where e.Level == ErrorLevel.Failure select e.Message, builder);
      }

      private static void Dump(string format, IEnumerable<string> content, StringBuilder builder)
      {
         if (!content.Any())
            return;
         builder.AppendFormat("{0} :\n", format);
         foreach (string error in content)
         {
            builder.AppendFormat("\"{0}\",\n", error);
         }
      }
   }
}
