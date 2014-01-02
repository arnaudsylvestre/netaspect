using System;
using System.Collections.Generic;

namespace FluentAspect.Weaver.Core.Configuration
{
   public interface IConfigurationReader
   {
      void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration);

   }
}