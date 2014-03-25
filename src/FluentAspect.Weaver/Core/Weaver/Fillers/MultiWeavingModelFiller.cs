using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weaver.Fillers
{
    public class MultiWeavingModelFiller : IWeavingModelFiller
    {
        private readonly IWeavingModelFiller[] weavingModelFillers;

        public MultiWeavingModelFiller(params IWeavingModelFiller[] weavingModelFillers_P)
        {
            weavingModelFillers = weavingModelFillers_P;
        }

        public bool CanHandle(NetAspectDefinition aspect)
        {
            return true;
        }

        public void FillWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            foreach (IWeavingModelFiller weavingModelFiller_L in weavingModelFillers)
            {
                if (weavingModelFiller_L.CanHandle(aspect))
                    weavingModelFiller_L.FillWeavingModel(method, aspect, weavingModel);
            }
        }
    }
}