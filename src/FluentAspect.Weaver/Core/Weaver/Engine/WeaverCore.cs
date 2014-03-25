using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Apis.AssemblyChecker;
using FluentAspect.Weaver.Apis.AssemblyChecker.Peverify;
using FluentAspect.Weaver.Core.Assemblies;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weaver.Engine
{
    public class WeaverCore
    {
        private readonly AroundMethodWeaver aroundMethodWeaver_L = new AroundMethodWeaver();
        private readonly WeavingModelComputer weavingModelComputer;
        IAssemblyChecker assemblyChecker = new PeVerifyAssemblyChecker();

        public WeaverCore(WeavingModelComputer weavingModelComputer_P)
        {
            weavingModelComputer = weavingModelComputer_P;
        }

        public void Weave(Type[] typesP_L, Type[] filter, ErrorHandler errorHandler, Func<string, string> newAssemblyNameProvider)
        {
            var assemblyPool = new AssemblyPool(assemblyChecker);

            foreach (var weavingModel in ComputeWeavingModels(typesP_L, filter, assemblyPool, errorHandler))
            {
                aroundMethodWeaver_L.Weave(new FluentAspect.Weaver.Helpers.IL.Method(weavingModel.Key), weavingModel.Value, errorHandler);
            }

            assemblyPool.Save(errorHandler, newAssemblyNameProvider);
        }

        private Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(Type[] typesP_L, Type[] filter, AssemblyPool assemblyPool, ErrorHandler errorHandler)
        {
            List<NetAspectDefinition> aspects = NetAspectDefinitionExtensions.FindAspects(typesP_L);
            CheckAspects(aspects, errorHandler);
            IEnumerable<Assembly> assembliesToWeave = aspects.GetAssembliesToWeave(typesP_L[0].Assembly);
            Dictionary<MethodDefinition, WeavingModel> weavingModels =
                weavingModelComputer.ComputeWeavingModels(assembliesToWeave, filter, assemblyPool, aspects);
            return weavingModels;
        }

        private void CheckAspects(IEnumerable<NetAspectDefinition> aspects, ErrorHandler errorHandler)
        {
            foreach (var aspect in aspects)
            {
                aspect.FieldSelector.Check(errorHandler);
            }
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