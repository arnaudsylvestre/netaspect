using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{
    public class CallUpdateFieldInterceptorAroundInstructionFactory : IInterceptorAroundInstructionFactory
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