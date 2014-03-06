using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes
{
    public class ConstructorAttributeConfigurationReader : IConfigurationReader
    {
        public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration,
                                      ErrorHandler errorHandler)
        {
            foreach (ConstructorInfo matchingMethod in types.GetAllConstructors((m) => true))
            {
                MethodBase info = matchingMethod;
                IEnumerable<NetAspectDefinition> methodWeavingAspectAttributes_L =
                    matchingMethod.GetNetAspectAttributes();
                foreach (NetAspectDefinition methodWeavingAspectAttribute_L in methodWeavingAspectAttributes_L)
                {
                    var errors = new List<string>();
                    configuration.AddConstructor(m => m.AreEqual(info),
                                                 new List<Assembly> {info.DeclaringType.Assembly},
                                                 methodWeavingAspectAttribute_L,
                                                 errors);
                    errorHandler.Errors.AddRange(errors);
                }
            }
        }
    }
}