using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Property
{
    public class CallGetPropertyInterceptorAroundInstructionBuilder : IInterceptorAroundInstructionBuilder
    {
        public void FillCommon(AroundInstructionInfo info)
        {
            info.AddCalled();
            info.AddCalledPropertyInfo();

            info.AddCaller();
            info.AddCallerParameters();
            info.AddCallerParameterNames();

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