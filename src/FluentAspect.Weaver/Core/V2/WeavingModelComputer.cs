﻿using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Helpers;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public class WeavingModelComputer
    {
        private readonly IWeavingModelFiller weavingModelFiller =
            new MultiWeavingModelFiller(new MethodAttributeWeavingModelFiller(),
                                        new PropertyGetAttributeWeavingModelFiller(),
                                        new CallMethodInstructionWeavingModelFiller() /*,
            new CallGetFieldInstructionWeavingModelFiller()*/);


        public Dictionary<MethodDefinition, WeavingModel> ComputeWeavingModels(IEnumerable<Assembly> assembliesToWeave,
                                                                               AssemblyPool assemblyDefinitionProvider,
                                                                               IEnumerable<NetAspectDefinition> aspects)
        {
            var weavingModels = new Dictionary<MethodDefinition, WeavingModel>();
            foreach (Assembly assembly_L in assembliesToWeave)
            {
                assemblyDefinitionProvider.Add(assembly_L);
                foreach (
                    MethodDefinition method in
                        assemblyDefinitionProvider.GetAssemblyDefinition(assembly_L).GetAllMethods())
                {
                    var model = new WeavingModel();
                    foreach (NetAspectDefinition aspect_L in aspects)
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