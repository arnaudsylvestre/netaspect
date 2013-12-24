using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Fluent.Helpers;

namespace FluentAspect.Weaver.Core.Fluent
{
   public class MethodAttributeConfigurationReader : IConfigurationReader
   {
       public void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration)
      {
           
          MethodBaseAttributeConfigurationReaderHelper.Fill(types.GetAllMethods(m => m.GetNetAspectAttributes(true).Count > 0).Cast<MethodBase>(), configuration.Methods);
      }
   }
}
