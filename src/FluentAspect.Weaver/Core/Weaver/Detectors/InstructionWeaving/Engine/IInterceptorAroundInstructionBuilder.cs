using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine
{
    public interface IInterceptorAroundInstructionBuilder
    {
        void FillCommon(AroundInstructionInfo info);
        void FillBeforeSpecific(AroundInstructionInfo info);
        void FillAfterSpecific(AroundInstructionInfo info);
    }
}