using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public interface IWeavingModelFiller
    {
        void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel);
    }
}