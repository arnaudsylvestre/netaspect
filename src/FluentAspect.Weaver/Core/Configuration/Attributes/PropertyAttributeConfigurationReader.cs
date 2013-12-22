using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
              configuration.Properties.Add(new PropertyMatch()
              {
                  InterceptorTypes = interceptorAttribute.ToList(),
                  Matcher = m => m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName
              });
          }

          return configuration;
      }
   }
}
