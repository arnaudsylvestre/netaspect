using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes
{
    public class FieldAttributeConfigurationReader : IConfigurationReader
    {
       public void ReadConfiguration(Assembly assembly, WeavingConfiguration configuration, ErrorHandler errorHandler)
        {
            foreach (var matchingMethod in assembly.GetTypes().GetAllFields((m) => true))
            {
                FieldInfo info = matchingMethod;
                foreach (var methodWeavingAspectAttribute_L in matchingMethod.GetNetAspectAttributes())
                {
                    var errors = new List<string>();
                   configuration.AddField(m => m.AreEqual(info),
                     new List<Assembly>() { info.DeclaringType.Assembly },
                     methodWeavingAspectAttribute_L, errors);
                    errorHandler.Errors.AddRange(errors);
                }
            }

                
        }
    }
}