using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Fluent.Helpers;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class MethodAttributeConfigurationReader : IConfigurationReader
   {
       public WeavingConfiguration ReadConfiguration(IEnumerable<Type> types)
      {
           
          var configuration = new WeavingConfiguration();

          MethodBaseAttributeConfigurationReaderHelper.Fill(types.GetAllMethods(m => m.GetNetAspectAttributes(true).Count > 0).Cast<MethodBase>(), configuration.Methods);

          return configuration;
      }
   }
}
