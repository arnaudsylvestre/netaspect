using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes
{
    public class PropertyAttributeConfigurationReader : IConfigurationReader
    {
        public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration,
                                      ErrorHandler errorHandler)
        {
            foreach (PropertyInfo matchingMethod in types.GetAllProperties((m) => true))
            {
                PropertyInfo info = matchingMethod;
                foreach (NetAspectDefinition methodWeavingAspectAttribute_L in matchingMethod.GetNetAspectAttributes())
                {
                    var errors = new List<string>();
                    configuration.AddProperty(m => m.AreEqual(info),
                                              new List<Assembly> {info.DeclaringType.Assembly},
                                              methodWeavingAspectAttribute_L, errors);
                    errorHandler.Errors.AddRange(errors);
                }
            }
        }
    }
}