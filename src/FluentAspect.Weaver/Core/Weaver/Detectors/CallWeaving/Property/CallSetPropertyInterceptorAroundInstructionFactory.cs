using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Property
{
    public class CallSetPropertyInterceptorAroundInstructionFactory : IInterceptorAroundInstructionFactory
    {
        public void FillCommon(InterceptorInfo info)
        {
            info.AddCalled();
            info.AddCalledFieldInfo();

            info.AddCaller();
            info.AddCallerParameters();
            info.AddCallerParameterNames();

            info.AddColumnNumber();
            info.AddLineNumber();
            info.AddFilePath();
            info.AddFileName();
        }

        public void FillBeforeSpecific(InterceptorInfo info)
        {
        }

        public void FillAfterSpecific(InterceptorInfo info)
        {
        }
    }
}