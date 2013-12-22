using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Fluent.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class ConstructorAttributeConfigurationReader : IConfigurationReader
   {
       public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
          var configuration = new WeavingConfiguration();

           MethodBaseAttributeConfigurationReaderHelper.Fill(types.GetAllConstructors(m => m.GetNetAspectAttributes(true).Count > 0).Cast<MethodBase>(), configuration.Constructors);

          return configuration;
      }
   }
}
