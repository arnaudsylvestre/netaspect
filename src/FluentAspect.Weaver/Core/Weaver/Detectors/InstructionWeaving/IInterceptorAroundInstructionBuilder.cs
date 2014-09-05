using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving
{
   public interface IInterceptorAroundInstructionBuilder
   {
      void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> parametersIlGenerator_P);
      void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> generator_P);
   }
}
