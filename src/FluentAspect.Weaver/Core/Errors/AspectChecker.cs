using System.Collections.Generic;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Core.Errors
{
    public class AspectChecker
    {
        public static void CheckAspects(ErrorHandler errorHandler, WeavingConfiguration weavingConfiguration)
        {
            Check(errorHandler, weavingConfiguration.Methods);
            Check(errorHandler, weavingConfiguration.Constructors);
        }

        private static void Check(ErrorHandler errorHandler, IEnumerable<AspectMatch<IMethod>> methodMatches)
        {
            foreach (var methodMatch in methodMatches)
            {
                CheckInterceptors(methodMatch.Aspect, errorHandler);
            }
        }

        private static void CheckInterceptors(NetAspectDefinition netAspectAttributes, ErrorHandler errorHandler)
        {
            if (netAspectAttributes == null)
                return;
            if (netAspectAttributes.After.Method == null &&
                netAspectAttributes.Before.Method == null &&
                netAspectAttributes.OnException.Method == null &&
                netAspectAttributes.OnFinally.Method == null &&
                netAspectAttributes.BeforeInterceptor.Method == null &&
                netAspectAttributes.AfterInterceptor.Method == null)
            {
                errorHandler.Warnings.Add(string.Format(
                    "The aspect {0} doesn't have a Before/After/OnException method", netAspectAttributes.Type.FullName));
            }
        }
    }
}