﻿using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Model
{
    public class NetAspectDefinition
    {
        private readonly Type _attribute;

        public NetAspectDefinition(Type attribute)
        {
            _attribute = attribute;
        }

        public IEnumerable<Assembly> AssembliesToWeave
        {
            get { return _attribute.GetValueForField<IEnumerable<Assembly>>("AssembliesToWeave", () => new Assembly[0]); }
        }

        public Type Type
        {
            get { return _attribute; }
        }

        public Interceptor Before
        {
            get { return new Interceptor(_attribute.GetMethod("Before")); }
        }

        public Interceptor After
        {
            get { return new Interceptor(_attribute.GetMethod("After")); }
        }

        public Interceptor OnException
        {
            get { return new Interceptor(_attribute.GetMethod("OnException")); }
        }

        public Interceptor OnFinally
        {
            get { return new Interceptor(_attribute.GetMethod("OnFinally")); }
        }

        public MethodInfo SelectorConstructor
        {
            get { return _attribute.GetMethod("WeaveConstructor"); }
        }

        public MethodInfo SelectorMethod
        {
            get { return _attribute.GetMethod("WeaveMethod"); }
        }

        public Interceptor BeforeInterceptor
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeCall")); }
        }

        public Interceptor AfterInterceptor
        {
            get { return new Interceptor(_attribute.GetMethod("AfterCall")); }
        }

        public Interceptor BeforeUpdateFieldValue
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeUpdateFieldValue")); }
        }

        public Interceptor AfterUpdateFieldValue
        {
            get { return new Interceptor(_attribute.GetMethod("AfterUpdateFieldValue")); }
        }

        public Interceptor BeforeGetField
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeGetField")); }
        }

        public Interceptor AfterGetField
        {
            get { return new Interceptor(_attribute.GetMethod("AfterGetField")); }
        }

        public bool IsValid
        {
            get
            {
                return Type.GetField("NetAspectAttribute",
                                     BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                     BindingFlags.Instance) != null;
            }
        }

        public Interceptor AfterPropertyGet
        {
            get { return new Interceptor(_attribute.GetMethod("AfterPropertyGet")); }
        }

        public Interceptor BeforePropertyGet
        {
            get { return new Interceptor(_attribute.GetMethod("BeforePropertyGet")); }
        }

        public Interceptor AfterPropertySet
        {
            get { return new Interceptor(_attribute.GetMethod("AfterPropertySet")); }
        }

        public Interceptor BeforePropertySet
        {
            get { return new Interceptor(_attribute.GetMethod("BeforePropertySet")); }
        }

        public Interceptor OnExceptionPropertySet
        {
            get { return new Interceptor(_attribute.GetMethod("OnExceptionPropertySet")); }
        }

        public Interceptor OnFinallyPropertySet
        {
            get { return new Interceptor(_attribute.GetMethod("OnFinallyPropertySet")); }
        }

        public Interceptor AfterParameter
        {
            get { return new Interceptor(_attribute.GetMethod("AfterParameter")); }
        }

        public Interceptor BeforeParameter
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeParameter")); }
        }

        public Interceptor OnExceptionParameter
        {
            get { return new Interceptor(_attribute.GetMethod("OnExceptionParameter")); }
        }

        public Interceptor AfterRaiseEvent
        {
            get { return new Interceptor(_attribute.GetMethod("AfterRaiseEvent")); }
        }

        public Interceptor BeforeRaiseEvent
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeRaiseEvent")); }
        }

        public Interceptor OnExceptionPropertyGet
        {
            get { return new Interceptor(_attribute.GetMethod("OnExceptionPropertyGet")); }
        }

        public Interceptor OnFinallyPropertyGet
        {
            get { return new Interceptor(_attribute.GetMethod("OnFinallyPropertyGet")); }
        }

        public Interceptor BeforeCallMethod
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeCallMethod")); }
        }

        public Interceptor AfterCallMethod
        {
            get { return new Interceptor(_attribute.GetMethod("AfterCallMethod")); }
        }

        public Selector FieldSelector
        {
            get { return new Selector(_attribute.GetMethod("SelectField")); } 
        }
    }

    public class SelectorParametersGenerator<T>
    {
        class PossibleParameter
        {
            public Type Type;
            public Func<T, object> Provider;
        }

        Dictionary<string, PossibleParameter> possibleParameters = new Dictionary<string, PossibleParameter>();

        public void AddPossibleParameter<TParameter>(string parameterName, Func<T, object> valueProvider)
        {
            possibleParameters.Add(parameterName.ToLower(), new PossibleParameter()
                {
                    Provider                    = valueProvider,
                    Type                    = typeof(TParameter)
                });
        }

        public object[] Generate(MethodInfo method, T data)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in method.GetParameters())
            {
                var possibleParameter = possibleParameters[parameterInfo.Name.ToLower()];
                if (possibleParameter.Type != parameterInfo.ParameterType)
            }
            return parameters.ToArray();
        }

        public void Check(MethodInfo method, ErrorHandler errorHandler)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in method.GetParameters())
            {
                var possibleParameter = possibleParameters[parameterInfo.Name.ToLower()];
                if (possibleParameter.Type != parameterInfo.ParameterType)
                    errorHandler.Errors.Add();
            }
            return parameters.ToArray();
        }
    }

    public class Selector
    {
        private readonly MethodInfo _method;

        public Selector(MethodInfo method)
        {
            _method = method;
        }

        public bool IsCompliant(FieldDefinition field)
        {


            var parameters = new List<object>();


            _method.Invoke(null, new object[])
        }
    }
}