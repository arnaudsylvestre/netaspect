using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Model.Helpers;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods
{
    public class AroundMethodWeaver : IWeaveable
    {
        private readonly MethodDefinition definition;
        private readonly IEnumerable<MethodWeavingConfiguration> interceptorType;

        public AroundMethodWeaver(IEnumerable<MethodWeavingConfiguration> interceptorType, MethodDefinition definition_P)
        {
            this.interceptorType = interceptorType;
            definition = definition_P;
        }

        public void Weave()
        {
            WeaveMethod(definition, interceptorType);
        }

        public void Check(ErrorHandler errorHandler)
        {
            if (!definition.HasBody)
            {
                if (definition.DeclaringType.IsInterface)
                    errorHandler.Errors.Add(string.Format("A method declared in interface can not be weaved : {0}.{1}",
                                                      definition.DeclaringType.Name, definition.Name));
                else if ((definition.Attributes & MethodAttributes.Abstract) == MethodAttributes.Abstract)
                    errorHandler.Errors.Add(string.Format("An abstract method can not be weaved : {0}.{1}",
                                                      definition.DeclaringType.Name, definition.Name));
            }
            foreach (var attribute in interceptorType)
            {
                CheckBeforeParameters(attribute.Before, errorHandler, definition);
                CheckAfterParameters(attribute.After, errorHandler, definition);
                CheckOnExceptionParameters(attribute.OnException, errorHandler, definition);
            }
        }

        public bool CanWeave()
        {
            foreach (var attribute in interceptorType)
            {
                if (attribute.Before.Method != null)
                    return true;
                if (attribute.After.Method != null)
                    return true;
                if (attribute.OnException.Method != null)
                    return true;
            }
            return false;
        }

        private void EnsureNotReferenced(ParameterInfo parameterInfo, ErrorHandler errorHandler)
        {
            if (parameterInfo.ParameterType.IsByRef)
            {
                errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter '{0}' in the method {1} of the type '{2}'", parameterInfo.Name, parameterInfo.Member.Name, parameterInfo.Member.DeclaringType.FullName));
            }
        }

        private void CheckOnExceptionParameters(Interceptor onExceptionInterceptor, ErrorHandler errorHandler, MethodDefinition methodDefinition)
        {
            var checkers = new Dictionary<string, Action<ParameterInfo, ErrorHandler>>();
            AddCommonCheckers(checkers, methodDefinition);
            checkers.Add("exception", (info, handler) =>
            {
                EnsureOfType(info, handler, typeof(Exception).FullName);
            });
            Check(errorHandler, onExceptionInterceptor, checkers);
        }

        private static void Check(ErrorHandler errorHandler, Interceptor interceptor, Dictionary<string, Action<ParameterInfo, ErrorHandler>> checkers)
        {
            foreach (var parameterInfo in interceptor.GetParameters())
            {
                if (!checkers.ContainsKey(parameterInfo.Name))
                {
                    errorHandler.Errors.Add(string.Format("The parameter {0} is not supported", parameterInfo.Name));
                }
                else
                {
                    checkers[parameterInfo.Name](parameterInfo, errorHandler);
                }
            }
        }

        private void CheckAfterParameters(Interceptor after, ErrorHandler errorHandler, MethodDefinition methodDefinition)
        {
            var checkers = new Dictionary<string, Action<ParameterInfo, ErrorHandler>>();
            AddCommonCheckers(checkers, methodDefinition);
            checkers.Add("result", (info, handler) =>
                {
                    EnsureResultOfType(info, handler, methodDefinition);
                });
            Check(errorHandler, after, checkers);
        }

        private void EnsureResultOfType(ParameterInfo info, ErrorHandler handler, MethodDefinition method)
        {
            if (info.ParameterType.FullName.Replace("&", "") != method.ReturnType.FullName)
            {
                handler.Errors.Add(string.Format("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4} because the return type of the method {5} in the type {6}",
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName, info.ParameterType.FullName,
                    method.ReturnType.FullName, method.Name, method.DeclaringType.FullName));
            }
        }



        private void EnsureOfType(ParameterInfo info, ErrorHandler handler, params string[] types)
        {
            if (!(from t in types where info.ParameterType.FullName.Replace("&", "") == t select t).Any())
            {
                handler.Errors.Add(string.Format("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4}",
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName, info.ParameterType.FullName,
                    string.Join(" or ", types)));
            }
        }
        private void EnsureOfType<T>(ParameterInfo info, ErrorHandler handler)
        {
            EnsureOfType(info, handler, typeof (T).FullName);
        }



        private void EnsureOfType(ParameterInfo info, ErrorHandler handler, ParameterDefinition parameter)
        {
            if (info.ParameterType == typeof(object))
                return;
            if (info.ParameterType.FullName.Replace("&", "") != parameter.ParameterType.FullName.Replace("&", ""))
            {
                handler.Errors.Add(string.Format("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4} because of the type of this parameter in the method {5} of the type {6}",
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName, info.ParameterType.FullName,
                    parameter.ParameterType.FullName, ((IMemberDefinition)parameter.Method).Name, ((IMemberDefinition)parameter.Method).DeclaringType.FullName));
            }
        }

        private void CheckBeforeParameters(Interceptor before, ErrorHandler errorHandler, MethodDefinition methodDefinition)
        {
            var checkers = new Dictionary<string, Action<ParameterInfo, ErrorHandler>>();
            AddCommonCheckers(checkers, methodDefinition);
            Check(errorHandler, before, checkers);
        }

        private void AddCommonCheckers(Dictionary<string, Action<ParameterInfo, ErrorHandler>> checkers, MethodDefinition methodDefinition)
        {
            checkers.Add("parameters", (p, handler) => { EnsureNotReferenced(p, handler);
                                                           EnsureOfType<object[]>(p, handler);
            });
            checkers.Add("instance", (p, handler) =>
                {
                    EnsureNotReferenced(p, handler);
                    EnsureOfType(p, handler, typeof(object).FullName, methodDefinition.DeclaringType.FullName);
                });
            checkers.Add("method", (p, handler) =>
            {
                EnsureNotReferenced(p, handler);
                EnsureOfType(p, handler, typeof(MethodInfo).FullName);
            });
            foreach (var parameter in methodDefinition.Parameters)
            {
                checkers.Add(parameter.Name, (p, handler) => { EnsureOfType(p, handler, parameter); });
                
            }
        }


        public static void WeaveMethod(MethodDefinition methodDefinition, IEnumerable<MethodWeavingConfiguration> interceptorTypes)
        {
            MethodDefinition newMethod = CreateNewMethodBasedOnMethodToWeave(methodDefinition, interceptorTypes);
            methodDefinition.DeclaringType.Methods.Add(newMethod);
        }

        private static MethodDefinition CreateNewMethodBasedOnMethodToWeave(MethodDefinition methodDefinition,
                                                                            IEnumerable<MethodWeavingConfiguration> interceptor)
        {
            MethodDefinition wrappedMethod = methodDefinition.Clone("-Weaved-" + methodDefinition.Name);

            methodDefinition.Body.Instructions.Clear();
            methodDefinition.Body.Variables.Clear();

           var weaver = new WeavedMethodBuilder();
            weaver.Build(new MethodToWeave(interceptor.ToList(), new Method(methodDefinition)),
                         wrappedMethod);
            methodDefinition.Body.InitLocals = true;
            return wrappedMethod;
        }
    }
}