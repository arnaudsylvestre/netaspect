namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{
    public interface IInterceptorAroundInstructionFactory
    {
        void FillCommon(InterceptorInfo info);
        void FillBeforeSpecific(InterceptorInfo info);
        void FillAfterSpecific(InterceptorInfo info);
    }
}