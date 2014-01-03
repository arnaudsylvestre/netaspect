using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Selector
{
    public class SelectorConfigurationReader : IConfigurationReader
    {
        public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration)
        {

            var assemblies = new HashSet<Assembly>();

            foreach (var type in types)
            {
                assemblies.Add(type.Assembly);
            }

            foreach (var assembly in assemblies)
            {
                var attrbiutes = assembly.GetNetAspectAttributes(true);
                foreach (var attrbiute in attrbiutes)
                {
                    var methodInfo_L = attrbiute.SelectorMethod;
                   if (methodInfo_L != null)
                    {
                        configuration.Methods.Add(new MethodMatch()
                            {
                               Interceptors = new List<NetAspectAttribute>() { attrbiute },
                                Matcher                                = m => CheckMethod(m, methodInfo_L)
                            });
                    }
                }
            }
        }

        private bool CheckMethod(IMethod arg, MethodInfo info)
        {
            List<object> parameters = new List<object>();

            var p = new Dictionary<string, object>
                {
                    {"methodName", arg.Name},
                    {"declaringTypeName", arg.DeclaringType.Name},
                    {"declaringTypeFullName", arg.DeclaringType.FullName}
                };

            foreach (var parameterInfo in info.GetParameters())
            {
                parameters.Add(p[parameterInfo.Name]);
            }

            return (bool) info.Invoke(null, parameters.ToArray());
        }
    }
}