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

        public void Weave(string assemblyFilePath, ErrorHandler errorHandler,
                          Func<string, string> newAssemblyNameProvider)
        {
            var toWeave = new List<Assembly>();
            Assembly mainAssembly = Assembly.LoadFrom(assemblyFilePath);
            List<NetAspectAttribute> netAspectAttributes = mainAssembly.GetNetAspectAttributes(true);
            toWeave.Add(mainAssembly);
            foreach (NetAspectAttribute netAspectAttribute in netAspectAttributes)
            {
                toWeave.AddRange(netAspectAttribute.AssembliesToWeave);
            }
            var weavingConfiguration = new WeavingConfiguration();
            foreach (Assembly asmToWeave in toWeave)
            {
                configurationReader.ReadConfiguration(Assembly.LoadFrom(asmToWeave.GetAssemblyPath()).GetTypes(),
                                                      weavingConfiguration);
            }

            foreach (Assembly asmToWeave in toWeave)
            {
                WeaveOneAssembly(asmToWeave.GetAssemblyPath(), errorHandler, newAssemblyNameProvider,
                                 weavingConfiguration);
            }
        }

        private void WeaveOneAssembly(string assemblyFilePath, ErrorHandler errorHandler,
                                      Func<string, string> newAssemblyNameProvider, WeavingConfiguration configuration)
        {
            AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFilePath,
                                                                                    new ReaderParameters(
                                                                                        ReadingMode.Immediate)
                                                                                        {
                                                                                            ReadSymbols = true
                                                                                        });
            IEnumerable<IWeaveable> weavers = weaverBuilder.BuildWeavers(assemblyDefinition, configuration);
            foreach (IWeaveable weaver_L in weavers)
            {
                try
                {
                    var error = new ErrorHandler();
                    weaver_L.Check(error);
                    if (error.Errors.Count == 0)
                        weaver_L.Weave(errorHandler);
                    errorHandler.Errors.AddRange(error.Errors);
                    errorHandler.Warnings.AddRange(error.Warnings);
                }
                catch (Exception e)
                {
                    errorHandler.Failures.Add(e.Message);
                }
            }
            string targetFileName = newAssemblyNameProvider(assemblyFilePath);
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

    internal class WeavingWarningException : Exception
    {
    }
}