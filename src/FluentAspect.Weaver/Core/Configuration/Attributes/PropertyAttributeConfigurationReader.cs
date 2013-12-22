using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class PropertyAttributeConfigurationReader : IConfigurationReader
   {
       private List<ConstructorInfo> constructorsWithAttributesToDelete = new List<ConstructorInfo>(); 

      public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
          var constructors = types.GetAllConstructors(m => m.GetNetAspectAttributes(true).Count > 0);
          constructorsWithAttributesToDelete.AddRange(constructors);
          var configuration = new WeavingConfiguration();

          foreach (var matchingMethod in constructors)
          {

              var interceptorAttribute = from m in matchingMethod.GetNetAspectAttributes(true) select (Type)m.GetType();
              var info = matchingMethod;
              configuration.Constructors.Add(new MethodMatch()
              {
                  InterceptorTypes = interceptorAttribute.ToList(),
                  Matcher = m => m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName
              });
          }

          return configuration;
      }

       public void Clean(AssemblyDefinition assemblyDefinition)
      {
      }
   }
}
