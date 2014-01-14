using System;
using System.Reflection;
using System.Runtime.Serialization;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core
{
    [Serializable]
    public class WeaverCore : ISerializable
    {
        private readonly IConfigurationReader configurationReader;
        private readonly IWeaverBuilder weaverBuilder;

        public WeaverCore(SerializationInfo info, StreamingContext context)
        {
        }

        public WeaverCore(IConfigurationReader configurationReader_P, IWeaverBuilder weaverBuilder_P)
        {
            configurationReader = configurationReader_P;
            weaverBuilder = weaverBuilder_P;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

       public class AssemblyInfo
       {
          public Assembly Assembly { get; set; }
          public AssemblyDefinition AssemblyDefinition { get; set; } 
       }

       public void Weave(string assemblyFilePath, ErrorHandler errorHandler,
                          Func<string, string> newAssemblyNameProvider)
        {
            Assembly mainAssembly = Assembly.LoadFrom(assemblyFilePath);
           var assemblyDefinitionProvider_L = new AssemblyDefinitionProvider();
           var weavingConfiguration = new WeavingConfiguration(assemblyDefinitionProvider_L);
            configurationReader.ReadConfiguration(mainAssembly, weavingConfiguration, errorHandler);
            AspectChecker.CheckAspects(errorHandler, weavingConfiguration);

           foreach (IWeaveable weaver_L in weaverBuilder.BuildWeavers(weavingConfiguration))
            {
                try
                {
                    var error = new ErrorHandler();
                    weaver_L.Check(error);
                    if (error.Errors.Count == 0 && weaver_L.CanWeave())
                        weaver_L.Weave(errorHandler);
                    errorHandler.Errors.AddRange(error.Errors);
                    errorHandler.Warnings.AddRange(error.Warnings);
                }
                catch (Exception e)
                {
                    errorHandler.Failures.Add(e.Message);
                }
            }
           foreach (var def in assemblyDefinitionProvider_L.Asms)
           {
               WeaveOneAssembly(def.Key.GetAssemblyPath(), def.Value, errorHandler, newAssemblyNameProvider);

           }
           foreach (var def in assemblyDefinitionProvider_L.Asms)
           {
               CheckAssembly(def.Key.GetAssemblyPath(), errorHandler);

           }
        }


       private void WeaveOneAssembly(string getAssemblyPath, AssemblyDefinition assemblyDefinition, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            string targetFileName = newAssemblyNameProvider(getAssemblyPath);
            assemblyDefinition.Write(targetFileName, new WriterParameters
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
                peVerify.Run(targetFileName);
            }
            catch (Exception e)
            {
                errorHandler.Errors.Add("An internal error has occured : " + e.Message);
            }
        }
    }
}