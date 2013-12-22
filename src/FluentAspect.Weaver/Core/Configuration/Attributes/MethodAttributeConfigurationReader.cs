using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class MethodAttributeConfigurationReader : IConfigurationReader
   {
       private List<MethodInfo> methodsWithAttributesToDelete = new List<MethodInfo>();

      public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
          var matchingMethods = types.GetAllMethods(m => m.GetNetAspectAttributes(true).Count > 0);
          methodsWithAttributesToDelete.AddRange(matchingMethods);
          var configuration = new WeavingConfiguration();

          foreach (var matchingMethod in matchingMethods)
          {

              var interceptorAttribute = from m in matchingMethod.GetNetAspectAttributes(true) select (Type)m.GetType();
              MethodInfo info = matchingMethod;
              configuration.Methods.Add(new MethodMatch()
              {
                  AdviceName = interceptorAttribute.ToList(),
                  Matcher = m => m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName
              });
          }

          return configuration;
      }

       public void Clean(AssemblyDefinition assemblyDefinition)
      {
           //var methodDefinitions = assemblyDefinition.GetAllMethods(m => true);
           //foreach (var methodInfo in methodsWithAttributesToDelete)
           //{
           //    MethodInfo info = methodInfo;
           //    var methodDefinition = (from m in methodDefinitions where m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName select m).FirstOrDefault();
           //    if (methodDefinition == null)
           //        continue;
           //    var methodInterceptorAttributes = info.GetNetAspectAttributes(false);
           //    foreach (var methodAttribute in methodInterceptorAttributes)
           //    {
           //        var attribute = methodAttribute;
           //        var attributesToDelete = (from a in methodDefinition.CustomAttributes where a.AttributeType.FullName == attribute.GetType().FullName select a).ToList();
           //        foreach (var customAttribute in attributesToDelete)
           //        {
           //            methodDefinition.CustomAttributes.Remove(customAttribute);
           //        }

           //    }
           //}
           //foreach (var moduleDefinition in assemblyDefinition.Modules)
           //{
           //    List<TypeDefinition> toDelete = new List<TypeDefinition>();
           //    foreach (var typeDefinition in moduleDefinition.GetTypes())
           //    {
           //        if (typeDefinition.BaseType == null)
           //            continue;
           //        if (typeDefinition.BaseType.FullName == typeof(MethodInterceptorAttribute).FullName)
           //        {
           //            toDelete.Add(typeDefinition);
           //        }
           //    }
           //    foreach (var typeDefinition in toDelete)
           //    {
           //        moduleDefinition.Types.Remove(typeDefinition);
           //    }
           //}
      }
   }
}
