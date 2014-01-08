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
                   MethodWeavingConfiguration attribute_L = attribute;
                if (attribute.SelectorMethod != null)
                {
                   configuration.AddMethod(m => CheckMethod(m, attribute_L.SelectorMethod), GetAssemblies(attribute, assembly), attribute, null);
                }
               if (attribute.SelectorConstructor != null)
                {
                   configuration.AddConstructor(m => CheckMethod(m, attribute_L.SelectorConstructor), GetAssemblies(attribute, assembly), attribute, null);
                }
            }
            foreach (var attribute in assembly.GetCallWeavingAspectAttributes())
            {
                   CallWeavingConfiguration attribute_L = attribute;
                if (attribute.SelectorMethod != null)
                {
                   configuration.AddMethod(m => CheckMethod(m, attribute_L.SelectorMethod), GetAssemblies(attribute, assembly), null, attribute);
                }
               if (attribute.SelectorConstructor != null)
                {
                   configuration.AddConstructor(m => CheckMethod(m, attribute_L.SelectorConstructor), GetAssemblies(attribute, assembly), null, attribute);
                }
            }
        }

        private IEnumerable<Assembly> GetAssemblies(MethodWeavingConfiguration attribute_P, Assembly assembly_P)
        {
           if (!attribute_P.AssembliesToWeave.Any())
              return new List<Assembly> { assembly_P };
           return attribute_P.AssembliesToWeave;
        }

        private IEnumerable<Assembly> GetAssemblies(CallWeavingConfiguration attribute_P, Assembly assembly_P)
        {
           if (!attribute_P.AssembliesToWeave.Any())
              return new List<Assembly> { assembly_P };
           return attribute_P.AssembliesToWeave;
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