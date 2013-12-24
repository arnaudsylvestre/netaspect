using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
   public interface IConfigurationReader
   {
      void ReadConfiguration(IEnumerable<Type> types, WeavingConfiguration configuration);

   }
}