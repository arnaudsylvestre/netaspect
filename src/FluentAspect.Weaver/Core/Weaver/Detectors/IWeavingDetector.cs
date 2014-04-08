﻿using Mono.Cecil;
using NetAspect.Weaver.Core.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{
    public interface IWeavingDetector
    {
        bool CanHandle(NetAspectDefinition aspect);
        void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel);
    }
}