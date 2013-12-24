using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Boo.Lang.Compiler.Steps;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Fluent;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
    [Serializable]
   public class WeaverCore : ISerializable
   {
      private IConfigurationReader configurationReader;
      private IWeaverBuilder weaverBuilder;

        public WeaverCore(SerializationInfo info, StreamingContext context)
        {
            
        }

      public WeaverCore(IConfigurationReader configurationReader_P, IWeaverBuilder weaverBuilder_P)
      {
         configurationReader = configurationReader_P;
         weaverBuilder = weaverBuilder_P;
      }

      public void Weave(string assemblyFilePath, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
      {
          var toWeave = new List<Assembly>();
          var mainAssembly = Assembly.LoadFrom(assemblyFilePath);
          var netAspectAttributes = mainAssembly.GetNetAspectAttributes(true);
          toWeave.Add(mainAssembly);
          foreach (var netAspectAttribute in netAspectAttributes)
          {
              var propertyInfo = netAspectAttribute.GetType().GetProperty("AssembliesToWeave");
              if (propertyInfo == null)
                  continue;
              toWeave.AddRange((IEnumerable<Assembly>) propertyInfo.GetValue(netAspectAttribute, new object[0]));
          }
          foreach (var asmToWeave in toWeave)
          {
              WeaveOneAssembly(asmToWeave.GetAssemblyPath(), errorHandler, newAssemblyNameProvider);
              
          }
      }

      private void WeaveOneAssembly(string assemblyFilePath, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            var configuration_L = new WeavingConfiguration();
            configurationReader.ReadConfiguration(Assembly.LoadFrom(assemblyFilePath).GetTypes(), configuration_L);
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFilePath,
                                                                     new ReaderParameters(ReadingMode.Immediate)
                                                                         {
                                                                             ReadSymbols = true
                                                                         });
            var weavers = weaverBuilder.BuildWeavers(assemblyDefinition, configuration_L, errorHandler);
            foreach (var weaver_L in weavers)
            {
                try
                {
                    weaver_L.Weave();
                }
                catch (Exception e)
                {
                    errorHandler.Warnings.Add(e.Message);
                }
            }
            var targetFileName = newAssemblyNameProvider(assemblyFilePath);
            assemblyDefinition.Write(targetFileName, new WriterParameters()
                {
                    WriteSymbols = true,
                });
            CheckAssembly(targetFileName, errorHandler);
        }

        private void CheckAssembly(string targetFileName, ErrorHandler errorHandler)
        {
            try
            {
                var peVerify = new PEVerify();
                //peVerify.Run(targetFileName);
            }
            catch (Exception e)
            {
                errorHandler.Errors.Add("An internal error has occured : " + e.Message);
            }

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }
   }
}


            