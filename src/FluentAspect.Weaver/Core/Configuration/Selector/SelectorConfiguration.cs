using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Selector
{
    public class SelectorConfigurationReader : IConfigurationReader
    {
        public void ReadConfiguration(Assembly assembly, WeavingConfiguration configuration, ErrorHandler errorHandler)
        {
            foreach (MethodWeavingConfiguration attribute in assembly.GetMethodWeavingAspectAttributes())
            {
                if (attribute.SelectorMethod != null)
                {
                    configuration.AddMethod(m => CheckMethod(m, attribute.SelectorMethod), attribute.AssembliesToWeave, new List<MethodWeavingConfiguration> { attribute }, new List<CallWeavingConfiguration>());
                }
                if (attribute.SelectorConstructor != null)
                {
                    configuration.AddConstructor(m => CheckMethod(m, attribute.SelectorConstructor), attribute.AssembliesToWeave, new List<MethodWeavingConfiguration> { attribute }, new List<CallWeavingConfiguration>());
                }
            }
            foreach (var attribute in assembly.GetCallWeavingAspectAttributes())
            {
                if (attribute.SelectorMethod != null)
                {
                    configuration.AddMethod(m => CheckMethod(m, attribute.SelectorMethod), attribute.AssembliesToWeave, new List<MethodWeavingConfiguration> { }, new List<CallWeavingConfiguration>() { attribute });
                }
                if (attribute.SelectorConstructor != null)
                {
                    configuration.AddConstructor(m => CheckMethod(m, attribute.SelectorConstructor), attribute.AssembliesToWeave, new List<MethodWeavingConfiguration> { }, new List<CallWeavingConfiguration>() { attribute });
                }
            }
        }

        private bool CheckMethod(IMethod arg, MethodInfo info)
        {
            var parameters = new List<object>();

            var p = new Dictionary<string, object>
                {
                    {"methodName", arg.Name},
                    {"declaringTypeName", arg.DeclaringType.Name},
                    {"declaringTypeFullName", arg.DeclaringType.FullName}
                };

            foreach (ParameterInfo parameterInfo in info.GetParameters())
            {
                parameters.Add(p[parameterInfo.Name]);
            }

            return (bool) info.Invoke(null, parameters.ToArray());
        }
    }
}