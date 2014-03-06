using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2
{
    public class MultiWeavingModelFiller : IWeavingModelFiller
    {
        private IWeavingModelFiller[] weavingModelFillers;

        public MultiWeavingModelFiller(params IWeavingModelFiller[] weavingModelFillers_P)
        {
            weavingModelFillers = weavingModelFillers_P;
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            foreach (var weavingModelFiller_L in weavingModelFillers)
            {
                weavingModelFiller_L.FillWeavingModel(method, aspect, weavingModel);
            }
        }
    }
}