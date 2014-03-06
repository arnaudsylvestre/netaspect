using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.Helpers
{
    public static class ErrorHandlerExtensions
    {
        public static void Dump(this ErrorHandler errorHandler, StringBuilder builder)
        {
            Dump("Warnings", errorHandler.Warnings, builder);
            Dump("Errors", errorHandler.Errors, builder);
            Dump("Failures", errorHandler.Failures, builder);
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