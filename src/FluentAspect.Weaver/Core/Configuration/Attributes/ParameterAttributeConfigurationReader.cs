using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes
{
    public class ParameterAttributeConfigurationReader : IConfigurationReader
    {
        public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration,
                                      ErrorHandler errorHandler)
        {
            foreach (ParameterInfo matchingMethod in types.GetAllParameters((m) => true))
            {
                ParameterInfo info = matchingMethod;
                foreach (NetAspectDefinition methodWeavingAspectAttribute_L in matchingMethod.GetNetAspectAttributes())
                {
                    var errors = new List<string>();
                    configuration.AddParameter(m => m.AreEqual(info),
                                               new List<Assembly> {info.Member.DeclaringType.Assembly},
                                               methodWeavingAspectAttribute_L,
                                               errors);
                    errorHandler.Errors.AddRange(errors);
                }
            }
        }
    }
}