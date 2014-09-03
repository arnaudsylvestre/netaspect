using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving
{
   public interface IInterceptorAroundInstructionBuilder
   {
      void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations parametersIlGenerator_P);
      void FillBeforeSpecific(InstructionWeavingInfo weavingInfo_P);
      void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations generator_P);
   }
}
