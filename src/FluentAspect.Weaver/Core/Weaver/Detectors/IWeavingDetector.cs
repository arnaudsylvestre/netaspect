using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{
    public interface IWeavingDetector
    {
        bool CanHandle(NetAspectDefinition aspect);
        void DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect, MethodWeavingModel methodWeavingModel);
    }

    public interface IMethodWeavingDetector
    {
        AroundMethodWeavingModel DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect);
    }

    public interface ICallWeavingDetector
    {
        IAroundInstructionWeaver DetectWeavingModel(MethodDefinition method, Instruction instruction, NetAspectDefinition aspect);
    }
}