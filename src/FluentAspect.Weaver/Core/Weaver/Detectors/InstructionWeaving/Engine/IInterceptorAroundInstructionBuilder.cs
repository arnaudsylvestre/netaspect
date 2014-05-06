using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine
{
    public interface IInterceptorAroundInstructionBuilder
    {
        void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> parametersIlGenerator_P);
        void FillBeforeSpecific(InstructionWeavingInfo weavingInfo_P);
        void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction> generator_P);
    }
}