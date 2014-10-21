using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Collections.Generic;
using NetAspect.Weaver.Core.Helpers;
using NetAspect.Weaver.Core.Selectors;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Helpers.NetFramework;

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
         get { return new Interceptor(_attribute.GetMethod("BeforeMethod")); }
      }

      public Interceptor After
      {
         get { return new Interceptor(_attribute.GetMethod("AfterMethod")); }
      }

      public Interceptor OnException
      {
         get { return new Interceptor(_attribute.GetMethod("OnExceptionMethod")); }
      }

      public Interceptor OnFinally
      {
         get { return new Interceptor(_attribute.GetMethod("OnFinallyMethod")); }
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
         get { return new Interceptor(_attribute.GetMethod("AfterUpdateProperty")); }
      }

      public Interceptor BeforeSetProperty
      {
         get { return new Interceptor(_attribute.GetMethod("BeforeUpdateProperty")); }
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
             return new Selector<FieldDefinition>(_attribute, "SelectField", new SelectorParametersGenerator<FieldDefinition>("field", GetField, typeof(FieldInfo)));
         }
      }

      public Selector<PropertyDefinition> PropertySelector
      {
         get
         {
             return new Selector<PropertyDefinition>(_attribute, "SelectProperty", new SelectorParametersGenerator<PropertyDefinition>("property", GetProperty, typeof(PropertyInfo)));
         }
      }

      public Selector<MethodDefinition> MethodSelector
      {
         get
         {
             return new Selector<MethodDefinition>(_attribute, "SelectMethod", new SelectorParametersGenerator<MethodDefinition>("method", GetMethod, typeof(MethodInfo)));
         }
      }

      public Selector<MethodDefinition> ConstructorSelector
      {
         get
         {
             return new Selector<MethodDefinition>(_attribute, "SelectConstructor", new SelectorParametersGenerator<MethodDefinition>("constructor", GetConstructor, typeof(ConstructorInfo)));
         }
      }

      public Selector<ParameterDefinition> ParameterSelector
      {
         get
         {
             return new Selector<ParameterDefinition>(_attribute, "SelectParameter", new SelectorParametersGenerator<ParameterDefinition>("parameter", GetParameter, typeof(ParameterInfo)));
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

      private PropertyInfo GetProperty(PropertyDefinition arg)
      {
         Assembly assembly = Assembly.LoadFrom(arg.Module.FullyQualifiedName);
         return assembly.GetType(arg.DeclaringType.FullName.Replace("/", "+")).GetProperty(arg.Name, ObjectExtensions.BINDING_FLAGS);
      }

      private MethodInfo GetMethod(MethodReference arg)
      {
         Assembly assembly = Assembly.LoadFrom(arg.Module.FullyQualifiedName);
         return assembly.GetType(arg.DeclaringType.FullName.Replace("/", "+")).GetMethod(arg.Name, ObjectExtensions.BINDING_FLAGS, null, ConvertParameters(arg.Parameters), new ParameterModifier[0]);
      }

      private ConstructorInfo GetConstructor(MethodDefinition arg)
      {
         Assembly assembly = Assembly.LoadFrom(arg.Module.FullyQualifiedName);
         return assembly.GetType(arg.DeclaringType.FullName.Replace("/", "+")).GetConstructor(ObjectExtensions.BINDING_FLAGS, null, ConvertParameters(arg.Parameters), new ParameterModifier[0]);
      }

      private ParameterInfo GetParameter(ParameterDefinition arg)
      {
         MethodInfo method = GetMethod((MethodReference) arg.Method);
         return method.GetParameters().First(p => p.Name == arg.Name);
      }

      private Type[] ConvertParameters(Collection<ParameterDefinition> parameters_P)
      {
         var types = new List<Type>();
         foreach (ParameterDefinition parameterDefinition_L in parameters_P)
         {
            types.Add(Type.GetType(parameterDefinition_L.ParameterType.FullName.Replace("/", "+")));
         }
         return types.ToArray();
      }

      private FieldInfo GetField(FieldDefinition arg)
      {
         Assembly assembly = Assembly.LoadFrom(arg.Module.FullyQualifiedName);
         return assembly.GetType(arg.DeclaringType.FullName.Replace("/", "+")).GetField(arg.Name, ObjectExtensions.BINDING_FLAGS);
      }
   }
}
