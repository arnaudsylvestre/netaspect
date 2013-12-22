using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class PropertyAttributeConfigurationReader : IConfigurationReader
   {

      public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
          var constructors = types.GetAllProperties(m => m.GetNetAspectAttributes(true).Count > 0);
          var configuration = new WeavingConfiguration();

          foreach (var matchingMethod in constructors)
          {

              var interceptorAttribute = from m in matchingMethod.GetNetAspectAttributes(true) select (Type)m.GetType();
              var info = matchingMethod;
              configuration.Methods.Add(new MethodMatch()
              {
                  InterceptorTypes = interceptorAttribute.ToList(),
                  Matcher = m => m.Name == "get_" + info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName
              });
              configuration.Methods.Add(new MethodMatch()
              {
                  InterceptorTypes = interceptorAttribute.ToList(),
                  Matcher = m => m.Name == "set_" + info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName
              });
          }

          return configuration;
      }

       public void Clean(AssemblyDefinition assemblyDefinition)
      {
      }
   }
}
