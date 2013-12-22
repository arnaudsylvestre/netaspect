using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAspect.Core;
using FluentAspect.Core.Attributes;
using FluentAspect.Weaver.Weavers.Methods;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class ConstructorAttributeConfigurationReader : IConfigurationReader
   {
       private List<ConstructorInfo> constructorsWithAttributesToDelete = new List<ConstructorInfo>(); 

      public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
          var constructors = types.GetAllConstructors(m => m.GetCustomAttributes<MethodInterceptorAttribute>(true).Count > 0);
          constructorsWithAttributesToDelete.AddRange(constructors);
          var configuration = new WeavingConfiguration();

          foreach (var matchingMethod in constructors)
          {
              var interceptorAttribute = from m in matchingMethod.GetCustomAttributes<MethodInterceptorAttribute>(true) select m.InterceptorType;
              var info = matchingMethod;
              configuration.Constructors.Add(new ConstructorMatch()
              {
                  AdviceName = interceptorAttribute.ToList(),
                  Matcher = m => m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName
              });
          }

          return configuration;
      }

       public void Clean(AssemblyDefinition assemblyDefinition)
      {
           var methodDefinitions = assemblyDefinition.GetAllConstructors(m => true);
           foreach (var methodInfo in constructorsWithAttributesToDelete)
           {
               var info = methodInfo;
               var methodDefinition = (from m in methodDefinitions where IsMethodEqual(m, info) select m).FirstOrDefault();
               if (methodDefinition == null)
                   continue;
               var methodInterceptorAttributes = info.GetCustomAttributes<MethodInterceptorAttribute>(false);
               foreach (var methodAttribute in methodInterceptorAttributes)
               {
                   MethodInterceptorAttribute attribute = methodAttribute;
                   var attributesToDelete = (from a in methodDefinition.CustomAttributes where a.AttributeType.FullName == attribute.GetType().FullName select a).ToList();
                   foreach (var customAttribute in attributesToDelete)
                   {
                       methodDefinition.CustomAttributes.Remove(customAttribute);
                   }

               }
           }
           foreach (var moduleDefinition in assemblyDefinition.Modules)
           {
               List<TypeDefinition> toDelete = new List<TypeDefinition>();
               foreach (var typeDefinition in moduleDefinition.GetTypes())
               {
                   if (typeDefinition.BaseType == null)
                       continue;
                   if (typeDefinition.BaseType.FullName == typeof(MethodInterceptorAttribute).FullName)
                   {
                       toDelete.Add(typeDefinition);
                   }
               }
               foreach (var typeDefinition in toDelete)
               {
                   moduleDefinition.Types.Remove(typeDefinition);
               }
           }
      }

       private static bool IsMethodEqual(MethodDefinition m, ConstructorInfo info)
       {
           var parameters = (from t in info.GetParameters() select t.ParameterType).ToList();
           var parametersMono = (from t in m.Parameters select t.ParameterType).ToList();
           if (parameters.Count() != parametersMono.Count())
               return false;
           for (int i = 0; i < parameters.Count(); i++)
           {
               if (parameters[i].FullName != parametersMono[i].FullName)
                   return false;
           }

           return m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName;
       }
   }
}
