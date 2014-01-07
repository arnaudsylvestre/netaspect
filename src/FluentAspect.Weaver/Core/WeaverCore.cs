using System;
using System.Collections.Generic;
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

        Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>(); 

        public void Weave(string assemblyFilePath, ErrorHandler errorHandler,
                          Func<string, string> newAssemblyNameProvider)
        {
            Assembly mainAssembly = Assembly.LoadFrom(assemblyFilePath);
            var weavingConfiguration = new WeavingConfiguration(new AssemblyDefinitionProvider());
            configurationReader.ReadConfiguration(mainAssembly, weavingConfiguration, errorHandler);
            foreach (var methodMatch in weavingConfiguration.Methods)
            {
                AspectChecker.CheckInterceptors(methodMatch.MethodWeavingInterceptors, errorHandler);
            }
            foreach (var methodMatch in weavingConfiguration.Constructors)
            {
                AspectChecker.CheckInterceptors(methodMatch.MethodWeavingInterceptors, errorHandler);
            }

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
            foreach (var def in asms)
            {
                WeaveOneAssembly(def.Key.GetAssemblyPath(), def.Value, errorHandler, newAssemblyNameProvider);
                
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
                //peVerify.Run(targetFileName);
            }
            catch (Exception e)
            {
                errorHandler.Errors.Add("An internal error has occured : " + e.Message);
            }
        }
    }

    public class AssemblyDefinitionProvider : IAssemblyDefinitionProvider
    {
        Dictionary<Assembly, AssemblyDefinition> asms = new Dictionary<Assembly, AssemblyDefinition>();

        public Dictionary<Assembly, AssemblyDefinition> Asms
        {
            get { return asms; }
        }

        public AssemblyDefinition GetAssemblyDefinition(Assembly assembly)
        {
            if (!asms.ContainsKey(assembly))
            {
                var parameters = new ReaderParameters(ReadingMode.Immediate)
                {
                    ReadSymbols = true
                };
                var asmDefinition = AssemblyDefinition.ReadAssembly(assembly.GetAssemblyPath(), parameters);
                asms.Add(assembly, asmDefinition);
            }
            return asms[assembly];
        }
    }
}