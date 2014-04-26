namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public interface IInterceptorAroundInstructionFactory
    {
        void FillCommon(InterceptorInfo info);
        void FillBeforeSpecific(InterceptorInfo info);
        void FillAfterSpecific(InterceptorInfo info);
    }
}