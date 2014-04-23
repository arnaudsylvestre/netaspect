using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Detectors.Selectors;
using NetAspect.Weaver.Helpers;

namespace NetAspect.Weaver.Core.Model.Aspect
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

        public Interceptor BeforeUpdateField
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeUpdateField")); }
        }

        public Interceptor AfterUpdateField
        {
            get { return new Interceptor(_attribute.GetMethod("AfterUpdateField")); }
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

        public Interceptor AfterGetProperty
        {
            get { return new Interceptor(_attribute.GetMethod("AfterGetProperty")); }
        }

        public Interceptor BeforeGetProperty
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeGetProperty")); }
        }

        public Interceptor AfterSetProperty
        {
            get { return new Interceptor(_attribute.GetMethod("AfterSetProperty")); }
        }

        public Interceptor BeforeSetProperty
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeSetProperty")); }
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

       public Interceptor BeforeCallMethod
        {
            get { return new Interceptor(_attribute.GetMethod("BeforeCallMethod")); }
        }

        public Interceptor AfterCallMethod
        {
            get { return new Interceptor(_attribute.GetMethod("AfterCallMethod")); }
        }

        public Interceptor BeforeConstructor
        {
           get { return new Interceptor(_attribute.GetMethod("BeforeConstructor")); }
        }

        public Interceptor AfterConstructor
        {
           get { return new Interceptor(_attribute.GetMethod("AfterConstructor")); }
        }

        public Interceptor OnFinallyConstructor
        {
           get { return new Interceptor(_attribute.GetMethod("OnFinallyConstructor")); }
        }

        public Interceptor OnExceptionConstructor
        {
           get { return new Interceptor(_attribute.GetMethod("OnExceptionConstructor")); }
        }

        public Selector<FieldDefinition> FieldSelector
        {
           get
           {
              var selectorParametersGenerator = new SelectorParametersGenerator<FieldDefinition>();
              selectorParametersGenerator.AddPossibleParameter<string>("fieldName", field => field.Name);
              selectorParametersGenerator.AddPossibleParameter<string>("fieldTypeName", field => field.FieldType.Name);
              return new Selector<FieldDefinition>(_attribute.GetMethod("SelectField"), selectorParametersGenerator);
           }
        }

        public Selector<PropertyDefinition> PropertySelector
        {
           get
           {
              var selectorParametersGenerator = new SelectorParametersGenerator<PropertyDefinition>();
              selectorParametersGenerator.AddPossibleParameter<string>("propertyName", field => field.Name);
              selectorParametersGenerator.AddPossibleParameter<string>("propertyTypeName", field => field.PropertyType.Name);
              return new Selector<PropertyDefinition>(_attribute.GetMethod("SelectProperty"), selectorParametersGenerator);
           }
        }

        public Selector<MethodDefinition> MethodSelector
        {
           get
           {
              var selectorParametersGenerator = new SelectorParametersGenerator<MethodDefinition>();
              selectorParametersGenerator.AddPossibleParameter<string>("methodName", field => field.Name);
              selectorParametersGenerator.AddPossibleParameter<string>("methodTypeName", field => field.ReturnType.Name);
              return new Selector<MethodDefinition>(_attribute.GetMethod("SelectMethod"), selectorParametersGenerator);
           }
        }

        public Selector<MethodDefinition> ConstructorSelector
        {
           get
           {
              var selectorParametersGenerator = new SelectorParametersGenerator<MethodDefinition>();
              selectorParametersGenerator.AddPossibleParameter<string>("constructorName", field => field.Name);
              return new Selector<MethodDefinition>(_attribute.GetMethod("SelectConstructor"), selectorParametersGenerator);
           }
        }

        public Interceptor BeforePropertyGetMethod
        {
            get { return new Interceptor(_attribute.GetMethod("BeforePropertyGetMethod")); }
        }

        public Interceptor AfterPropertyGetMethod
        {
            get { return new Interceptor(_attribute.GetMethod("AfterPropertyGetMethod")); }
        }

        public Interceptor OnExceptionPropertyGetMethod
        {
            get { return new Interceptor(_attribute.GetMethod("OnExceptionPropertyGetMethod")); }
        }

        public Interceptor OnFinallyPropertyGetMethod
        {
            get { return new Interceptor(_attribute.GetMethod("OnFinallyPropertyGetMethod")); }
        }

        public Interceptor BeforePropertySetMethod
        {
            get { return new Interceptor(_attribute.GetMethod("BeforePropertySetMethod")); }
        }

        public Interceptor AfterPropertySetMethod
        {
            get { return new Interceptor(_attribute.GetMethod("AfterPropertySetMethod")); }
        }

        public Interceptor OnExceptionPropertySetMethod
        {
            get { return new Interceptor(_attribute.GetMethod("OnExceptionPropertySetMethod")); }
        }

        public Interceptor OnFinallyPropertySetMethod
        {
            get { return new Interceptor(_attribute.GetMethod("OnFinallyPropertySetMethod")); }
        }

       public LifeCycle LifeCycle
       {
          get { return LifeCycleHelper.Convert(_attribute.GetValueForField("LifeCycle", () => "Transient")); }
       }
    }
}