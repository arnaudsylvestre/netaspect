using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Configuration.Attributes.Helpers;
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
                configuration.AddConstructor(m => m.AreEqual(info),
                    new List<Assembly>() { info.DeclaringType.Assembly },
                    matchingMethod.GetMethodWeavingAspectAttributes(),
                    matchingMethod.GetCallWeavingAspectAttributes());
            }
        }
    }
}