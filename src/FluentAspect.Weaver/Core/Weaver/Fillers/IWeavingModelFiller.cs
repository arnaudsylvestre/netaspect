using Mono.Cecil;
using NetAspect.Weaver.Core.Model;

namespace NetAspect.Weaver.Core.Weaver.Fillers
{
    public interface IWeavingModelFiller
    {
        bool CanHandle(NetAspectDefinition aspect);
        void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel);
    }
}