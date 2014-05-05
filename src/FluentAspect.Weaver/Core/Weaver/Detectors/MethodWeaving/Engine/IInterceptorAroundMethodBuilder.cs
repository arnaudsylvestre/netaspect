namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine
{
    public interface IInterceptorAroundMethodBuilder
    {
        void FillCommon(AroundMethodInfo info);
        void FillBeforeSpecific(AroundMethodInfo info);
        void FillAfterSpecific(AroundMethodInfo info);
        void FillOnExceptionSpecific(AroundMethodInfo info);
        void FillOnFinallySpecific(AroundMethodInfo info);
    }
}