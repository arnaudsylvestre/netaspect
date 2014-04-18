using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Assemblies;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Method;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Property;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
    public class WeavingModelComputer
    {
        private readonly IWeavingDetector _weavingDetector =
            new MultiWeavingDetector(new MethodAttributeWeavingDetector(),
                                        new PropertyGetAttributeWeavingDetector(),
                                        new CallMethodInstructionWeavingDetector(),
            new CallGetFieldInstructionWeavingDetector(), new CallUpdateFieldInstructionWeavingDetector(),
            new CallGetPropertyInstructionWeavingDetector(), new ConstructorAttributeWeavingDetector());


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
                        _weavingDetector.DetectWeavingModel(method, aspect_L, model);
                    }
                    if (!model.IsEmpty)
                        weavingModels.Add(method, model);
                }
            }
            return weavingModels;
        }
    }
}