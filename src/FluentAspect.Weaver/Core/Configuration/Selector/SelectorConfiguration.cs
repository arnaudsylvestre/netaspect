using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Core.Methods;

namespace FluentAspect.Weaver.Core.Selector
{
    public class SelectorConfigurationReader : IConfigurationReader
    {
        public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
        {
            var configuration = new WeavingConfiguration();

            var assemblies = new HashSet<Assembly>();

            foreach (var type in types)
            {
                assemblies.Add(type.Assembly);
            }

            foreach (var assembly in assemblies)
            {
                var attrbiutes = from a in assembly.GetCustomAttributes(true)
                                 where a.GetType().Name.EndsWith("NetAspectAttribute")
                                 select a;
                foreach (var attrbiute in attrbiutes)
                {
                    var type = attrbiute.GetType();
                    if (type.GetMethod("WeaveMethod") != null)
                    {
                        configuration.Methods.Add(new MethodMatch()
                            {
                                InterceptorTypes = new List<Type>() {type},
                                Matcher                                = m => CheckMethod(m, type.GetMethod("WeaveMethod"))
                            });
                    }
                }
            }

            return configuration;
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