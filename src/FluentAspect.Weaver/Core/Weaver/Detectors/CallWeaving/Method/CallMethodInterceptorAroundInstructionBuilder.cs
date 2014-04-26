using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Method
{
    public class CallMethodInterceptorAroundInstructionBuilder : IInterceptorAroundInstructionBuilder
    {
        public void FillCommon(AroundInstructionInfo info)
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

        public void FillBeforeSpecific(AroundInstructionInfo info)
        {
        }

        public void FillAfterSpecific(AroundInstructionInfo info)
        {
        }
    }
}