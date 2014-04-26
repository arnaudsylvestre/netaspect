namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public interface IInterceptorAroundInstructionBuilder
    {
        void FillCommon(AroundInstructionInfo info);
        void FillBeforeSpecific(AroundInstructionInfo info);
        void FillAfterSpecific(AroundInstructionInfo info);
    }
}