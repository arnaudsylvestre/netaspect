using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Field
{
    public class CallGetFieldInterceptorAroundInstructionBuilder : IInterceptorAroundInstructionBuilder
    {
        public void FillCommon(AroundInstructionInfo info)
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

        public void FillBeforeSpecific(AroundInstructionInfo info)
        {
        }

        public void FillAfterSpecific(AroundInstructionInfo info)
        {
        }
    }
}