using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentAspect.Core;
using FluentAspect.Core.Attributes;
using FluentAspect.Weaver.Weavers.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class AttributeConfigurationReader : IConfigurationReader
   {
      public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
          var matchingMethods = GetAllMatchingMethods(types);
          WeavingConfiguration configuration = new WeavingConfiguration();

          foreach (var matchingMethod in matchingMethods)
          {
              var interceptorAttribute = customAttribute as MethodInterceptorAttribute;
              MethodInfo info = matchingMethod;
              configuration.Methods.Add(new MethodMatch()
              {
                  AdviceName = interceptorAttribute.InterceptorType,
                  Matcher = m => m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName
              });
          }

          return configuration;
      }

       private static List<MethodInfo> GetAllMatchingMethods(IEnumerable<Type> types)
       {
           List<MethodInfo> matchingMethods = new List<MethodInfo>();
           foreach (var type in types)
           {
               foreach (var methodInfo in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic))
               {
                   var customAttributes = methodInfo.GetCustomAttributes(false);
                   foreach (var customAttribute in customAttributes)
                   {
                       if (customAttribute is MethodInterceptorAttribute)
                       {
                           matchingMethods.Add(methodInfo);
                       }
                   }
               }
           }
           return matchingMethods;
       }

       public void Clean(AssemblyDefinition assemblyDefinition)
      {

          var matchingMethods = GetAllMatchingMethods(assemblyDefinition);
          
      }

       private List<MethodDefinition> GetAllMatchingMethods(AssemblyDefinition assemblyDefinition)
       {
           List<MethodDefinition> matchingMethods = new List<MethodDefinition>();
           
           foreach (var methodInfo in assemblyDefinition.GetAllMethods())
           {
               var customAttributes = methodInfo.CustomAttributes;
               foreach (var customAttribute in customAttributes)
               {
                   if (customAttribute.AttributeType.Resolve().Is(typeof(MethodInterceptorAttribute)))
                   {
                       matchingMethods.Add(methodInfo);
                   }
               }
           }
           
           return matchingMethods;
       }
   }
}
