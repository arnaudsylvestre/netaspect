using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Configuration.Attributes.Helpers;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes
{
   public class ConstructorAttributeConfigurationReader : IConfigurationReader
   {
       public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration)
      {
           MethodBaseAttributeConfigurationReaderHelper.Fill(types.GetAllConstructors(m => m.GetNetAspectAttributes(true).Count > 0).Cast<MethodBase>(), configuration.Constructors);

      }
   }
}
