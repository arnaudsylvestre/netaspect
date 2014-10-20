using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Instruction.Detector
{
   public interface IInterceptorParameterConfigurationForInstructionFiller
   {
      void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> parametersIlGenerator_P);
      void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> generator_P);
   }
}
