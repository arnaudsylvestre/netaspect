using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FluentAspect.Weaver.Core.Configuration;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model.Adapters;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core
{


   public class IlMethodInjector
   {
       public AssemblyDefinition Assembly { get { return Method.DeclaringType.Module.Assembly; } }

       public MethodReference Method { get; private set; }

       public IlMethodInjector(MethodDefinition method)
       {
           Method = method;
       }

       public void Weave()
       {
           
       }
       
   }

    public class MethodWeavingInjectorFiller : IInjectorFiller
    {
        public void AddInjectors(IlMethodInjector methodInjector, WeavingConfiguration configuration)
        {
            foreach (var aspectMatch in configuration.Methods)
            {
                if (aspectMatch.AssembliesToScan.Contains(methodInjector.Assembly))
                {
                    if (aspectMatch.Matcher(new MethodDefinitionAdapter(methodInjector.Method)) &&
                        aspectMatch.Aspect != null)
                    {
                        
                    }
                }
            }
        }
    }

    public interface IInjectorFiller
    {
        void AddInjectors(IlMethodInjector methodInjector, WeavingConfiguration configuration);
    }

    public class MethodWeavingBuilder
    {
        private List<IInjectorFiller> fillers;

        public MethodWeavingBuilder(List<IInjectorFiller> fillers)
        {
            this.fillers = fillers;
        }

        public IlMethodInjector Compute(MethodDefinition definition_P, WeavingConfiguration configuration_P)
      {
          var methodInjector = new IlMethodInjector(definition_P);
            foreach (var filler in fillers)
            {
                filler.AddInjectors(methodInjector, configuration_P);
            }
            return methodInjector;
      }
   }

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

        public void Weave(IEnumerable<Type> types, ErrorHandler errorHandler,
                          Func<string, string> newAssemblyNameProvider)
        {
            var assemblyDefinitionProvider_L = new AssemblyDefinitionProvider();
            var weavingConfiguration = new WeavingConfiguration(assemblyDefinitionProvider_L);
            configurationReader.ReadConfiguration(types, weavingConfiguration, errorHandler);
            AspectChecker.CheckAspects(errorHandler, weavingConfiguration);

           foreach (IWeaveable weaver_L in weaverBuilder.BuildWeavers(weavingConfiguration))
            {
                try
                {
                    var error = new ErrorHandler();
                    weaver_L.Check(error);
                    if (error.Errors.Count == 0)
                        weaver_L.Weave();
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

        public void Weave(string assemblyFilePath, ErrorHandler errorHandler,
                          Func<string, string> newAssemblyNameProvider)
        {
            Assembly mainAssembly = Assembly.LoadFrom(assemblyFilePath);
            var types = mainAssembly.GetTypes();
           Weave(types, errorHandler, newAssemblyNameProvider);
        }


       public static void WeaveOneAssembly(string getAssemblyPath, AssemblyDefinition assemblyDefinition, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            string targetFileName = newAssemblyNameProvider(getAssemblyPath);
            assemblyDefinition.Write(targetFileName, new WriterParameters
                {
                    WriteSymbols = true,
                });
            CheckAssembly(targetFileName, errorHandler);
        }

        public static void CheckAssembly(string targetFileName, ErrorHandler errorHandler)
        {
            try
            {
                PEVerify.Run(targetFileName);
            }
            catch (Exception e)
            {
                errorHandler.Errors.Add("An internal error has occured : " + e.Message);
            }
        }
    }
}