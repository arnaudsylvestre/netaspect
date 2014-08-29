using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Collections.Generic;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine.Selectors;
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

        public Interceptor BeforeCallConstructor
        {
           get { return new Interceptor(_attribute.GetMethod("BeforeCallConstructor")); }
        }

        public Interceptor AfterCallConstructor
        {
           get { return new Interceptor(_attribute.GetMethod("AfterCallConstructor")); }
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
              selectorParametersGenerator.AddPossibleParameter<FieldInfo>("field", GetField);
              return new Selector<FieldDefinition>(_attribute, "SelectField", selectorParametersGenerator);
           }
        }

        public Selector<PropertyDefinition> PropertySelector
        {
           get
           {
               var selectorParametersGenerator = new SelectorParametersGenerator<PropertyDefinition>();
               selectorParametersGenerator.AddPossibleParameter<PropertyInfo>("property", GetProperty);
              return new Selector<PropertyDefinition>(_attribute, "SelectProperty", selectorParametersGenerator);
           }
        }

        private PropertyInfo GetProperty(PropertyDefinition arg)
        {
           var assembly = Assembly.LoadFrom(arg.Module.FullyQualifiedName);
           return assembly.GetType(arg.DeclaringType.FullName.Replace("/", "+")).GetProperty(arg.Name);
        }
        private MethodInfo GetMethod(MethodReference arg)
        {
           var assembly = Assembly.LoadFrom(arg.Module.FullyQualifiedName);
           return assembly.GetType(arg.DeclaringType.FullName.Replace("/", "+")).GetMethod(arg.Name, ConvertParameters(arg.Parameters));
        }
        private ConstructorInfo GetConstructor(MethodDefinition arg)
        {
            var assembly = Assembly.LoadFrom(arg.Module.FullyQualifiedName);
            return assembly.GetType(arg.DeclaringType.FullName.Replace("/", "+")).GetConstructor(ConvertParameters(arg.Parameters));
        }
        private ParameterInfo GetParameter(ParameterDefinition arg)
        {
            var method = GetMethod((MethodReference)arg.Method);
            return method.GetParameters().First(p => p.Name == arg.Name);
        }

       private Type[] ConvertParameters(Collection<ParameterDefinition> parameters_P)
       {
          List<Type> types = new List<Type>();
          foreach (var parameterDefinition_L in parameters_P)
          {
             types.Add(Type.GetType(parameterDefinition_L.ParameterType.FullName.Replace("/", "+")));
          }
          return types.ToArray();
       }

       private FieldInfo GetField(FieldDefinition arg)
        {
           var assembly = Assembly.LoadFrom(arg.Module.FullyQualifiedName);
           return assembly.GetType(arg.DeclaringType.FullName.Replace("/", "+")).GetField(arg.Name);
        }

        public Selector<MethodDefinition> MethodSelector
        {
           get
           {
               var selectorParametersGenerator = new SelectorParametersGenerator<MethodDefinition>();
               selectorParametersGenerator.AddPossibleParameter<MethodInfo>("method", GetMethod);
              return new Selector<MethodDefinition>(_attribute, "SelectMethod", selectorParametersGenerator);
           }
        }

        public Selector<MethodDefinition> ConstructorSelector
        {
           get
           {
              var selectorParametersGenerator = new SelectorParametersGenerator<MethodDefinition>();
              selectorParametersGenerator.AddPossibleParameter<ConstructorInfo>("constructor", GetConstructor);
              return new Selector<MethodDefinition>(_attribute, "SelectConstructor", selectorParametersGenerator);
           }
        }
        public Selector<ParameterDefinition> ParameterSelector
        {
           get
           {
               var selectorParametersGenerator = new SelectorParametersGenerator<ParameterDefinition>();
               selectorParametersGenerator.AddPossibleParameter<ParameterInfo>("parameter", GetParameter);
              return new Selector<ParameterDefinition>(_attribute, "SelectParameter", selectorParametersGenerator);
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

       public Interceptor AfterMethodForParameter
       {
           get { return new Interceptor(_attribute.GetMethod("AfterMethodForParameter")); }
       }

       public Interceptor BeforeMethodForParameter
       {
           get { return new Interceptor(_attribute.GetMethod("BeforeMethodForParameter")); }
       }

       public Interceptor OnExceptionMethodForParameter
       {
           get { return new Interceptor(_attribute.GetMethod("OnExceptionMethodForParameter")); }
       }

       public Interceptor OnFinallyMethodForParameter
       {
           get { return new Interceptor(_attribute.GetMethod("OnFinallyMethodForParameter")); }
       }

       public Interceptor AfterConstructorForParameter
       {
           get { return new Interceptor(_attribute.GetMethod("AfterConstructorForParameter")); }
       }

       public Interceptor BeforeConstructorForParameter
       {
           get { return new Interceptor(_attribute.GetMethod("BeforeConstructorForParameter")); }
       }

       public Interceptor OnExceptionConstructorForParameter
       {
           get { return new Interceptor(_attribute.GetMethod("OnExceptionConstructorForParameter")); }
       }

       public Interceptor OnFinallyConstructorForParameter
       {
           get { return new Interceptor(_attribute.GetMethod("OnFinallyConstructorForParameter")); }
       }

    }
}