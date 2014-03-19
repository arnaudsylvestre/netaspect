using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.V2.Assemblies;
using FluentAspect.Weaver.Core.V2.Model;
using FluentAspect.Weaver.Core.V2.Weaver.Fillers;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Engine
{
    public class WeavingModelComputer
    {
        private readonly IWeavingModelFiller weavingModelFiller =
            new MultiWeavingModelFiller(new MethodAttributeWeavingModelFiller(),
                                        new PropertyGetAttributeWeavingModelFiller(),
                                        new CallMethodInstructionWeavingModelFiller(),
            new CallGetFieldInstructionWeavingModelFiller());


        public Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(IEnumerable<Assembly> assembliesToWeave, Type[] filter, AssemblyPool assemblyDefinitionProvider, IEnumerable<NetAspectDefinition> aspects)
        {
            var weavingModels = new Dictionary<MethodDefinition, WeavingModel>();
            foreach (Assembly assembly_L in assembliesToWeave)
            {
                assemblyDefinitionProvider.Add(assembly_L);
                foreach (var method in assemblyDefinitionProvider.GetAssemblyDefinition(assembly_L).GetAllMethods(filter))
                {
                    var model = new WeavingModel();
                    foreach (var aspect_L in aspects)
                    {
                        weavingModelFiller.FillWeavingModel(method, aspect_L, model);
                    }
                    if (!model.IsEmpty)
                        weavingModels.Add(method, model);
                }
            }
            return weavingModels;
        }
    }
}