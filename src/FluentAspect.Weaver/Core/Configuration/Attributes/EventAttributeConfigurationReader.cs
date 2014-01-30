using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes
{
    public class EventAttributeConfigurationReader : IConfigurationReader
    {
       public void ReadConfiguration(Assembly assembly, WeavingConfiguration configuration, ErrorHandler errorHandler)
        {
            foreach (var matchingMethod in assembly.GetTypes().GetAllEvents((m) => true))
            {
                var info = matchingMethod;
               var methodWeavingAspectAttributes_L = matchingMethod.GetNetAspectAttributes();
               foreach (var methodWeavingAspectAttribute_L in methodWeavingAspectAttributes_L)
               {
                   var errors = new List<string>();
                   configuration.AddEvent(m => m.AreEqual(info),
                    new List<Assembly>() { info.DeclaringType.Assembly },
                    methodWeavingAspectAttribute_L,
                    errors);
                   errorHandler.Errors.AddRange(errors);
               }
            }
        }
    }
}