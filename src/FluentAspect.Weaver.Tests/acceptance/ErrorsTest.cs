using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Tests.acceptance
{
    public static class ErrorsTest
    {

        public static void Dump(ErrorHandler errorHandler, StringBuilder builder)
        {
            Dump("Warnings", errorHandler.Warnings, builder);
            Dump("Errors", errorHandler.Errors, builder);
            Dump("Failures", errorHandler.Failures, builder);

        }

        private static void Dump(string format, IEnumerable<string> warnings, StringBuilder builder)
        {
            if (!warnings.Any())
                return;
            builder.AppendFormat("{0} :\n", format);
            foreach (var error in warnings)
            {
                builder.AppendFormat("\"{0}\",\n", error);
            }
        }
    }
}