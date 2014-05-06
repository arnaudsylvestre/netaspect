using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Field
{
    public class CallGetFieldInterceptorAroundInstructionBuilder : IInterceptorAroundInstructionBuilder
    {
       public void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> parametersIlGenerator_P)
        {
           weavingInfo_P.AddCalled(parametersIlGenerator_P)
                .AddCalledFieldInfo(parametersIlGenerator_P)
                
                .AddCaller(parametersIlGenerator_P)
                .AddCallerParameters(parametersIlGenerator_P)
                .AddCallerParameterNames(parametersIlGenerator_P)
                
                .AddColumnNumber(parametersIlGenerator_P)
                .AddLineNumber(parametersIlGenerator_P)
                .AddFilePath(parametersIlGenerator_P)
                .AddFileName(parametersIlGenerator_P);
        }

        public void FillBeforeSpecific(InstructionWeavingInfo weavingInfo_P)
        {
        }

        public void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> generator_P)
        {
        }
    }
}