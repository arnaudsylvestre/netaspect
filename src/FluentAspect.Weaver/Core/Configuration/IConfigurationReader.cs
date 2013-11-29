using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
   public interface IConfigurationReader
   {
      WeavingConfiguration ReadConfiguration(IEnumerable<Type> types);
      void Clean(AssemblyDefinition assemblyDefinitionP_P);

   }
}