using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes
{
    public class MethodAttributeConfigurationReader : IConfigurationReader
    {
       public void ReadConfiguration(Assembly assembly, WeavingConfiguration configuration, ErrorHandler errorHandler)
        {
            foreach (MethodInfo matchingMethod in assembly.GetTypes().GetAllMethods((m) => true))
            {
                MethodBase info = matchingMethod;
                foreach (var methodWeavingAspectAttribute_L in matchingMethod.GetMethodWeavingAspectAttributes())
                {
                   configuration.AddMethod(m => m.AreEqual(info),
                     new List<Assembly>() { info.DeclaringType.Assembly },
                     methodWeavingAspectAttribute_L,
                     null);
                }
                foreach (var methodWeavingAspectAttribute_L in matchingMethod.GetCallWeavingAspectAttributes())
                {
                   configuration.AddMethod(m => m.AreEqual(info),
                     new List<Assembly>() { info.DeclaringType.Assembly },
                     null,
                     methodWeavingAspectAttribute_L);
                }
            }

                
        }
    }
}