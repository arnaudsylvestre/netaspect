using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Property
{
    public class CallSetPropertyInterceptorAroundInstructionBuilder : IInterceptorAroundInstructionBuilder
    {
       public void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations parametersIlGenerator_P)
        {
            weavingInfo_P.AddCalled(parametersIlGenerator_P);
            weavingInfo_P.AddCalledFieldInfo(parametersIlGenerator_P);

            weavingInfo_P.AddCaller(parametersIlGenerator_P);
            weavingInfo_P.AddCallerParameters(parametersIlGenerator_P);
            weavingInfo_P.AddCallerParameterNames(parametersIlGenerator_P);

            weavingInfo_P.AddColumnNumber(parametersIlGenerator_P);
            weavingInfo_P.AddLineNumber(parametersIlGenerator_P);
            weavingInfo_P.AddFilePath(parametersIlGenerator_P);
            weavingInfo_P.AddFileName(parametersIlGenerator_P);
        }

        public void FillBeforeSpecific(InstructionWeavingInfo weavingInfo_P)
        {
        }

        public void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations generator_P)
        {
        }
    }
}