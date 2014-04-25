using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{

    public class WeavingDetector
    {
        public interface IMethodWeavingDetector
        {
            void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, AroundMethodWeavingModel methodWeavingModel);
        }

        public interface IInstructionWeavingDetector
        {
            
        }

        private IMethodWeavingDetector methodWeavingDetector;

        public WeavingDetector(IMethodWeavingDetector methodWeavingDetector)
        {
            this.methodWeavingDetector = methodWeavingDetector;
        }

        public bool CanHandle(NetAspectDefinition aspect)
        {
            throw new System.NotImplementedException();
        }

        public void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, MethodWeavingModel methodWeavingModel)
        {
            methodWeavingDetector.DetectWeavingModel(method, aspect, methodWeavingModel.Method);
        }
    }
}