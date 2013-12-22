using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Boo.Lang.Compiler.Steps;
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

        public WeaverCore(SerializationInfo info, StreamingContext context)
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
         Clean(assemblyDefinition);

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

        private void Clean(AssemblyDefinition assemblyDefinition)
        {
            configurationReader.Clean(assemblyDefinition);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }
   }
}


            