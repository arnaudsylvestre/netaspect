using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public class WeaverCore
    {
        private readonly AroundMethodWeaver aroundMethodWeaver_L = new AroundMethodWeaver();
        private readonly WeavingModelComputer weavingModelComputer;

        public WeaverCore(WeavingModelComputer weavingModelComputer_P)
        {
            weavingModelComputer = weavingModelComputer_P;
        }

        public void Weave(Type[] typesP_L, Type[] filter, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            var assemblyPool = new AssemblyPool();

            foreach (var weavingModel in ComputeWeavingModels(typesP_L, filter, assemblyPool))
            {
                aroundMethodWeaver_L.Weave(new Method(weavingModel.Key), weavingModel.Value, errorHandler);
            }

            assemblyPool.Save(errorHandler, newAssemblyNameProvider);
        }

        private Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(Type[] typesP_L, Type[] filter, AssemblyPool assemblyPool)
        {
            List<NetAspectDefinition> aspects = NetAspectDefinitionExtensions.FindAspects(typesP_L);
            IEnumerable<Assembly> assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
            Dictionary<MethodDefinition, WeavingModel> weavingModels =
                weavingModelComputer.ComputeWeavingModels(assembliesToWeave, filter, assemblyPool, aspects);
            return weavingModels;
        }

        public void Weave(string assemblyFilePath, ErrorHandler errorHandler,
                          Func<string, string> newAssemblyNameProvider)
        {
            Assembly mainAssembly = Assembly.LoadFrom(assemblyFilePath);
            Type[] types = mainAssembly.GetTypes();
            Weave(types, null, errorHandler, newAssemblyNameProvider);
        }
    }
}