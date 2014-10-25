using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Collections.Generic;
using NetAspect.Weaver.Core.Helpers;
using NetAspect.Weaver.Core.Selectors;
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

      public Interceptors BeforeMethod
      {
         get { return GetInterceptors("BeforeMethod"); }
      }

      public Interceptors AfterMethod
      {
         get { return GetInterceptors("AfterMethod"); }
      }

      public Interceptors OnExceptionMethod
      {
         get { return GetInterceptors("OnExceptionMethod"); }
      }

      public Interceptors OnFinallyMethod
      {
         get { return GetInterceptors("OnFinallyMethod"); }
      }

      public Interceptors BeforeUpdateField
      {
         get { return GetInterceptors("BeforeUpdateField"); }
      }

      public Interceptors AfterUpdateField
      {
         get { return GetInterceptors("AfterUpdateField"); }
      }

      public Interceptors BeforeGetField
      {
         get { return GetInterceptors("BeforeGetField"); }
      }

      public Interceptors AfterGetField
      {
         get { return GetInterceptors("AfterGetField"); }
      }


      public Interceptors AfterGetProperty
      {
         get { return GetInterceptors("AfterGetProperty"); }
      }

      public Interceptors BeforeGetProperty
      {
         get { return GetInterceptors("BeforeGetProperty"); }
      }

      public Interceptors AfterSetProperty
      {
         get { return GetInterceptors("AfterUpdateProperty"); }
      }

      public Interceptors BeforeSetProperty
      {
         get { return GetInterceptors("BeforeUpdateProperty"); }
      }

      public Interceptors BeforeCallMethod
      {
         get { return GetInterceptors("BeforeCallMethod"); }
      }

      public Interceptors AfterCallMethod
      {
         get { return GetInterceptors("AfterCallMethod"); }
      }

      public Interceptors BeforeCallConstructor
      {
         get { return GetInterceptors("BeforeCallConstructor"); }
      }

      public Interceptors AfterCallConstructor
      {
         get { return GetInterceptors("AfterCallConstructor"); }
      }

      public Interceptors BeforeConstructor
      {
         get { return GetInterceptors("BeforeConstructor"); }
      }

      public Interceptors AfterConstructor
      {
         get { return GetInterceptors("AfterConstructor"); }
      }

      public Interceptors OnFinallyConstructor
      {
         get { return GetInterceptors("OnFinallyConstructor"); }
      }

      public Interceptors OnExceptionConstructor
      {
         get { return GetInterceptors("OnExceptionConstructor"); }
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

      public Interceptors BeforePropertyGetMethod
      {
         get { return GetInterceptors("BeforePropertyGetMethod"); }
      }

      public Interceptors AfterPropertyGetMethod
      {
         get { return GetInterceptors("AfterPropertyGetMethod"); }
      }

      public Interceptors OnExceptionPropertyGetMethod
      {
         get { return GetInterceptors("OnExceptionPropertyGetMethod"); }
      }

      public Interceptors OnFinallyPropertyGetMethod
      {
         get { return GetInterceptors("OnFinallyPropertyGetMethod"); }
      }

      public Interceptors BeforePropertySetMethod
      {
         get { return GetInterceptors("BeforePropertySetMethod"); }
      }

      public Interceptors AfterPropertySetMethod
      {
         get { return GetInterceptors("AfterPropertySetMethod"); }
      }

      public Interceptors OnExceptionPropertySetMethod
      {
         get { return GetInterceptors("OnExceptionPropertySetMethod"); }
      }

      public Interceptors OnFinallyPropertySetMethod
      {
         get { return GetInterceptors("OnFinallyPropertySetMethod"); }
      }

      public LifeCycle LifeCycle
      {
         get { return LifeCycleHelper.Convert(_attribute.GetValueForField("LifeCycle", () => "Transient")); }
      }

      public Interceptors AfterMethodForParameter
      {
         get { return GetInterceptors("AfterMethodForParameter"); }
      }

      public Interceptors BeforeMethodForParameter
      {
          get { return GetInterceptors("BeforeMethodForParameter"); }
      }

       private Interceptors GetInterceptors(string interceptorName)
       {
           return new Interceptors(_attribute.GetMethods().Where(m => m.Name == interceptorName).ToList());
       }

       public Interceptors OnExceptionMethodForParameter
      {
         get { return GetInterceptors("OnExceptionMethodForParameter"); }
      }

       public Interceptors OnFinallyMethodForParameter
      {
         get { return GetInterceptors("OnFinallyMethodForParameter"); }
      }

      public Interceptors AfterConstructorForParameter
      {
         get { return GetInterceptors("AfterConstructorForParameter"); }
      }

      public Interceptors BeforeConstructorForParameter
      {
          get { return GetInterceptors("BeforeConstructorForParameter"); }
      }

      public Interceptors OnExceptionConstructorForParameter
      {
         get { return GetInterceptors("OnExceptionConstructorForParameter"); }
      }

      public Interceptors OnFinallyConstructorForParameter
      {
         get { return GetInterceptors("OnFinallyConstructorForParameter"); }
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
