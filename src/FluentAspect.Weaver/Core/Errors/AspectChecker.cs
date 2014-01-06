using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;

namespace FluentAspect.Weaver.Core.Errors
{
    public class AspectChecker
    {
         public static void CheckInterceptors(List<NetAspectAttribute> netAspectAttributes, ErrorHandler errorHandler)
        {
            foreach (var netAspectAttribute in netAspectAttributes)
            {
                if (netAspectAttribute.Kind == NetAspectAttributeKind.MethodWeaving)
                {
                    if (netAspectAttribute.MethodWeavingConfiguration.After.Method == null &&
                        netAspectAttribute.MethodWeavingConfiguration.Before.Method == null &&
                        netAspectAttribute.MethodWeavingConfiguration.OnException.Method == null)
                    {
                       errorHandler.Warnings.Add(string.Format("The aspect {0} doesn't have a Before/After/OnException method", netAspectAttribute.MethodWeavingConfiguration.Type.FullName));
                    }                        
                }
            }
        }
    }
}