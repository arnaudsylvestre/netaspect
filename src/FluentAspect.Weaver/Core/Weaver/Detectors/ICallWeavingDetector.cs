using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors
{
    public interface ICallWeavingDetector
    {
        IAroundInstructionWeaver DetectWeavingModel(MethodDefinition method, Instruction instruction, NetAspectDefinition aspect);
    }
}