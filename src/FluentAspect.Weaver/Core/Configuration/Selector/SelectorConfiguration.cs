using System;
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
                    var errors = new List<string>();
                    EnsureParameters(attribute_L.SelectorMethod, errors);
                    errorHandler.Errors.AddRange(errors);
                    if (errors.Count > 0)
                        continue;
                    errors = new List<string>();
                    configuration.AddMethod(m => CheckMethod(m, attribute_L.SelectorMethod), GetAssemblies(attribute, assembly), attribute, null, errors);
                    errorHandler.Errors.AddRange(errors);
                }
               if (attribute.SelectorConstructor != null)
                {
                    var errors = new List<string>();
                    EnsureParameters(attribute_L.SelectorMethod, errors);
                    errorHandler.Errors.AddRange(errors);
                    if (errors.Count > 0)
                        continue;
                    errors = new List<string>();
                    configuration.AddConstructor(m => CheckMethod(m, attribute_L.SelectorConstructor), GetAssemblies(attribute, assembly), attribute, null, errors);
                    errorHandler.Errors.AddRange(errors);
                }
            }
            foreach (var attribute in assembly.GetCallWeavingAspectAttributes())
            {
                   CallWeavingConfiguration attribute_L = attribute;
                if (attribute.SelectorMethod != null)
                {
                    var errors = new List<string>();
                    EnsureParameters(attribute_L.SelectorMethod, errors);
                    errorHandler.Errors.AddRange(errors);
                    if (errors.Count > 0)
                        continue;
                    errors = new List<string>();
                   configuration.AddMethod(m => CheckMethod(m, attribute_L.SelectorMethod), GetAssemblies(attribute, assembly), null, attribute, errors);
                    errorHandler.Errors.AddRange(errors);
                }
               if (attribute.SelectorConstructor != null)
                {
                    var errors = new List<string>();
                    EnsureParameters(attribute_L.SelectorMethod, errors);
                    errorHandler.Errors.AddRange(errors);
                    if (errors.Count > 0)
                        continue;
                    errors = new List<string>();
                    configuration.AddConstructor(m => CheckMethod(m, attribute_L.SelectorConstructor), GetAssemblies(attribute, assembly), null, attribute, errors);
                    errorHandler.Errors.AddRange(errors);
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


        private void EnsureParameters(MethodInfo methodInfo, List<string> errorHandler)
        {
            CreateFiller().Check(methodInfo, errorHandler);
        }

        public class Parameter
        {
            public string Name { get; set; }
            public Type Type { get; set; }
            public object Value { get; set; }
        }

        public class ParametersFiller
        {
            private Dictionary<string, Parameter> descriptions = new Dictionary<string, Parameter>();

            public void Add(Parameter parameter)
            {
                descriptions.Add(parameter.Name, parameter);
            }

            public void Check(MethodInfo info, List<string> handler)
            {
                foreach (ParameterInfo parameterInfo in info.GetParameters())
                {
                    var fullName = descriptions[parameterInfo.Name].Type.FullName;
                    if (fullName != parameterInfo.ParameterType.FullName)
                        handler.Add(string.Format("Wrong type for parameter {0}, expected {1}", parameterInfo.Name, fullName));
                }
            }

            public List<object> Fill(MethodInfo info)
            {
                var parameters = new List<object>();

                foreach (ParameterInfo parameterInfo in info.GetParameters())
                {
                    parameters.Add(descriptions[parameterInfo.Name].Value);
                }
                return parameters;
            }

            public void SetValue(string key, object value)
            {
                descriptions[key].Value = value;
            }
        }

       private bool CheckMethod(IMethod arg, MethodInfo info)
        {
            var filler = CreateFiller();
           filler.SetValue("methodName", arg.Name);
           var parameters = filler.Fill(info);

           return (bool) info.Invoke(null, parameters.ToArray());
        }

       private static ParametersFiller CreateFiller()
        {
            var filler = new ParametersFiller();
            filler.Add(new Parameter()
                {
                    Name = "methodName",
                    Type = typeof(string),
                });
            filler.Add(new Parameter()
                {
                    Name = "declaringTypeName",
                    Type = typeof(string),
                });
            filler.Add(new Parameter()
                {
                    Name = "declaringTypeFullName",
                    Type = typeof(string),
                });
            return filler;
        }
    }
}