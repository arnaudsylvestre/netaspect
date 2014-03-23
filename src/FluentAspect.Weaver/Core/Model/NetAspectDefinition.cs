using System;
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

        public Selector<FieldDefinition> FieldSelector
        {
            get
            {
                var selectorParametersGenerator = new SelectorParametersGenerator<FieldDefinition>();
                selectorParametersGenerator.AddPossibleParameter<string>("fieldName", field => field.Name);
                return new Selector<FieldDefinition>(_attribute.GetMethod("SelectField"), selectorParametersGenerator);
            }
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
                parameters.Add(possibleParameters[parameterInfo.Name.ToLower()].Provider(data));
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
                    errorHandler.Errors.Add("Type different");
            }
        }
    }

    public class Selector<T>
    {
        private readonly MethodInfo _method;
        private SelectorParametersGenerator<T> selectorParametersGenerator;

        public Selector(MethodInfo method, SelectorParametersGenerator<T> selectorParametersGenerator)
        {
            _method = method;
            this.selectorParametersGenerator = selectorParametersGenerator;
        }

        public void Check(ErrorHandler errorHandler)
        {
            if (_method == null)
                return;
            selectorParametersGenerator.Check(_method, errorHandler);
            if (!_method.IsStatic)
                errorHandler.OnError("The selector {0} in the aspect {1} must be static", _method.Name, _method.DeclaringType.FullName);

            if (_method.ReturnType != typeof(bool))
                errorHandler.OnError("The selector {0} in the aspect {1} must be return boolean value", _method.Name, _method.DeclaringType.FullName);
        }

        public bool IsCompliant(T member)
        {
            var errorHandler = new ErrorHandler();
            Check(errorHandler);
            if (errorHandler.Errors.Count > 0 || errorHandler.Failures.Count > 0)
                return false;
            return (bool)_method.Invoke(null, selectorParametersGenerator.Generate(_method, member));
        }
    }
}