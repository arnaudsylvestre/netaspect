using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Method
{
    public class CallMethodInterceptorAroundInstructionFactory : IInterceptorAroundInstructionFactory
    {
        public void FillCommon(InterceptorInfo info)
        {
            info.AddCalled();
            info.AddCalledParameters();
            info.AddCalledParameterNames();

            info.AddCaller();
            info.AddCallerParameters();
            info.AddCallerParameterNames();
            info.AddCallerMethod();

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