using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weaver.Fillers
{
    public interface IWeavingModelFiller
    {
        bool CanHandle(NetAspectDefinition aspect);
        void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel);
    }
}