using Mono.Cecil;
using NetAspect.Weaver.Core.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{
    public class MultiWeavingDetector : IWeavingDetector
    {
        private readonly IWeavingDetector[] _weavingDetectors;

        public MultiWeavingDetector(params IWeavingDetector[] weavingDetectorsP)
        {
            _weavingDetectors = weavingDetectorsP;
        }

        public bool CanHandle(NetAspectDefinition aspect)
        {
            return true;
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, WeavingModel weavingModel)
        {
            foreach (IWeavingDetector weavingModelFiller_L in _weavingDetectors)
            {
                if (weavingModelFiller_L.CanHandle(aspect))
                    weavingModelFiller_L.DetectWeavingModel(method, aspect, weavingModel);
            }
        }
    }
}