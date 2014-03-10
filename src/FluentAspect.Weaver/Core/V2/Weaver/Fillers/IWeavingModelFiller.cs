using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.V2.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Fillers
{
    public interface IWeavingModelFiller
    {
        bool CanHandle(NetAspectDefinition aspect);
        void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel);
    }
}