using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FluentAspect.Core.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Fluent;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
    [Serializable]
   public class WeaverCore : ISerializable
   {
      private IConfigurationReader configurationReader;
      private IWeaverBuilder weaverBuilder;

       public WeaverCore()
          : this(new FluentConfigurationReader(), new AroundMethodBuilderWeaver())
       {
           
       }

       public WeaverCore(SerializationInfo info, StreamingContext context)
           : this(new FluentConfigurationReader(), new AroundMethodBuilderWeaver())
        {
            
        }

      public WeaverCore(IConfigurationReader configurationReader_P, IWeaverBuilder weaverBuilder_P)
      {
         configurationReader = configurationReader_P;
         weaverBuilder = weaverBuilder_P;
      }

      public void Weave(string assemblyFilePath, string targetFileName, ErrorHandler errorHandler)
      {
         var configuration_L = configurationReader.ReadConfiguration(Assembly.LoadFrom(assemblyFilePath).GetTypes());
         var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFilePath, new ReaderParameters(ReadingMode.Immediate) { ReadSymbols = true });
         var weavers = weaverBuilder.BuildWeavers(assemblyDefinition, configuration_L);
         foreach (var weaver_L in weavers)
         {
            weaver_L.Weave();
         }
         Clean(assemblyDefinition);
         assemblyDefinition.Write(targetFileName, new WriterParameters()
         {
            WriteSymbols = true,
         });
      }

      private void Clean(AssemblyDefinition assemblyDefinition)
      {
         configurationReader.Clean(assemblyDefinition);
         CleanReferencesToNetAspect(assemblyDefinition);
      }

      private static void CleanReferencesToNetAspect(AssemblyDefinition assemblyDefinition)
      {
         foreach (var moduleDefinition in assemblyDefinition.Modules)
         {
            var same = (from r in moduleDefinition.AssemblyReferences where r.FullName == typeof(IInterceptor).Assembly.FullName select r).ToList();
            foreach (var reference in same)
            {
               moduleDefinition.AssemblyReferences.Remove(reference);
            }
             foreach (var typeDefinition in moduleDefinition.GetTypes())
             {
                 var interfaces = (from i in typeDefinition.Interfaces where i.FullName == typeof (IInterceptor).FullName select i).ToList();
                 foreach (var @interface in interfaces)
                 {
                     typeDefinition.Interfaces.Remove(@interface);
                 }
             }
         }


      }

       public void GetObjectData(SerializationInfo info, StreamingContext context)
       {
           
       }
   }
}
