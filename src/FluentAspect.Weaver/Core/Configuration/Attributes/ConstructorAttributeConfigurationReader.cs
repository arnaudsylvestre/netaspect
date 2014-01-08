using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes
{
    public class ConstructorAttributeConfigurationReader : IConfigurationReader
    {
       public void ReadConfiguration(Assembly assembly, WeavingConfiguration configuration, ErrorHandler errorHandler)
        {
            foreach (var matchingMethod in assembly.GetTypes().GetAllConstructors((m) => true))
            {
                MethodBase info = matchingMethod;
               var methodWeavingAspectAttributes_L = matchingMethod.GetMethodWeavingAspectAttributes();
               foreach (var methodWeavingAspectAttribute_L in methodWeavingAspectAttributes_L)
               {
                   var errors = new List<string>();
                   configuration.AddConstructor(m => m.AreEqual(info),
                    new List<Assembly>() { info.DeclaringType.Assembly },
                    methodWeavingAspectAttribute_L,
                    null, errors);
                   errorHandler.Errors.AddRange(errors);
               }
                foreach (var methodWeavingAspectAttribute_L in matchingMethod.GetCallWeavingAspectAttributes())
                {
                    var errors = new List<string>();
                  configuration.AddConstructor(m => m.AreEqual(info),
                    new List<Assembly>() { info.DeclaringType.Assembly },
                    null,
                    methodWeavingAspectAttribute_L, errors);
                  errorHandler.Errors.AddRange(errors);
               }
            }
        }
    }
}