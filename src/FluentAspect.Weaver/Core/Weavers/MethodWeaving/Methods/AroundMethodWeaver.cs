using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Core.Weavers.Methods
{
    public class AroundMethodWeaver : IWeaveable
    {
        private readonly MethodDefinition definition;
        private readonly List<NetAspectAttribute> interceptorType;

        public AroundMethodWeaver(List<NetAspectAttribute> interceptorType, MethodDefinition definition_P)
        {
            this.interceptorType = interceptorType;
            definition = definition_P;
        }

        public void Weave(ErrorHandler errorP_P)
        {
            Check(errorP_P);
            WeaveMethod(definition, interceptorType);
        }

        public void Check(ErrorHandler errorHandler)
        {
            if (!definition.HasBody)
            {
                if (definition.DeclaringType.IsInterface)
                    throw new Exception(string.Format("A method declared in interface can not be weaved : {0}.{1}",
                                                      definition.DeclaringType.Name, definition.Name));
                if ((definition.Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract)
                    throw new Exception(string.Format("An abstract method can not be weaved : {0}.{1}",
                                                      definition.DeclaringType.Name, definition.Name));
            }
            foreach (var attribute in interceptorType)
            {
                CheckBeforeParameters(attribute.MethodWeavingConfiguration.Before, errorHandler);
            }
        }

        private void CheckBeforeParameters(Interceptor before, ErrorHandler errorHandler)
        {
            var checkers = new Dictionary<string, Action<ParameterInfo, ErrorHandler>>();
            checkers.Add("parameters", (p, handler) =>
                {
                    
                });
            foreach (var parameterInfo in before.GetParameters())
            {
                if (!checkers.ContainsKey(parameterInfo.Name))
                {
                    //errorHandler.Errors.Add(string.Format("The parameter {0} is not supported", parameterInfo.Name));
                }
                else
                {
                    checkers[parameterInfo.Name](parameterInfo, errorHandler);
                }
            }
        }


        public static void WeaveMethod(MethodDefinition methodDefinition, List<NetAspectAttribute> interceptorTypes)
        {
            MethodDefinition newMethod = CreateNewMethodBasedOnMethodToWeave(methodDefinition, interceptorTypes);
            methodDefinition.DeclaringType.Methods.Add(newMethod);
        }

        private static MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition,
                                                                            List<NetAspectAttribute> interceptor)
        {
            MethodDefinition wrappedMethod = methodDefinition.Clone("-Weaved-" + methodDefinition.Name);

            methodDefinition.Body.Instructions.Clear();
            methodDefinition.Body.Variables.Clear();

            IEnumerable<MethodWeavingConfiguration> methodWeavingConfigurations = from i in interceptor
                                                                                  select i.MethodWeavingConfiguration;
            var weaver = new WeavedMethodBuilder();
            weaver.Build(new MethodToWeave(methodWeavingConfigurations.ToList(), new Method(methodDefinition)),
                         wrappedMethod);
            methodDefinition.Body.InitLocals = true;
            return wrappedMethod;
        }
    }
}