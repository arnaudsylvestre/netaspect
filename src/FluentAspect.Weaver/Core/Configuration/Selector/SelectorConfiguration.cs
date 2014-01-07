﻿using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Selector
{
    public class SelectorConfigurationReader : IConfigurationReader
    {
        public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration)
        {
            var assemblies = new HashSet<Assembly>();

            foreach (Type type in types)
            {
                assemblies.Add(type.Assembly);
            }

            foreach (Assembly assembly in assemblies)
            {
                List<MethodWeavingConfiguration> attrbiutes = assembly.GetMethodWeavingAspectAttributes(true);
                foreach (MethodWeavingConfiguration attrbiute in attrbiutes)
                {
                    MethodInfo methodInfo_L = attrbiute.SelectorMethod;
                    if (methodInfo_L != null)
                    {
                        configuration.Methods.Add(new MethodMatch
                            {
                               MethodWeavingInterceptors = new List<MethodWeavingConfiguration> { attrbiute },
                                Matcher = m => CheckMethod(m, methodInfo_L)
                            });
                    }
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