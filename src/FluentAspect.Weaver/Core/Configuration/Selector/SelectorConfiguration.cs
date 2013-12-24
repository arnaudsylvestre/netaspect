﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Core.Methods;
using FluentAspect.Weaver.Core.Fluent;

namespace FluentAspect.Weaver.Core.Selector
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