using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Core.Errors
{
    public class AspectChecker
    {
       public static void CheckInterceptors(IEnumerable<MethodWeavingConfiguration> netAspectAttributes, ErrorHandler errorHandler)
        {
            foreach (var netAspectAttribute in netAspectAttributes)
            {
                    if (netAspectAttribute.After.Method == null &&
                        netAspectAttribute.Before.Method == null &&
                        netAspectAttribute.OnException.Method == null)
                    {
                       errorHandler.Warnings.Add(string.Format("The aspect {0} doesn't have a Before/After/OnException method", netAspectAttribute.Type.FullName));
                    }        
            }
        }
    }
}