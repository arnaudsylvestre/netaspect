using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.Instruction.Detector;
using NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration.Instruction
{
   public class CallConstructorInterceptorParameterConfigurationForInstructionFiller : IInterceptorParameterConfigurationForInstructionFiller
   {
       public void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> parametersIlGenerator_P)
      {
         //weavingInfo_P.AddCalled(parametersIlGenerator_P, weavingInfo_P.GetOperandAsMethod());
          weavingInfo_P.AddCalledParameters(parametersIlGenerator_P);
          weavingInfo_P.AddCalledParameterNames(parametersIlGenerator_P);
          weavingInfo_P.AddCalledConstructorInfo(parametersIlGenerator_P);

         weavingInfo_P.AddCaller(parametersIlGenerator_P);
         weavingInfo_P.AddCallerParameters(parametersIlGenerator_P);
         weavingInfo_P.AddCallerMethod(parametersIlGenerator_P);

         weavingInfo_P.AddColumnNumber(parametersIlGenerator_P);
         weavingInfo_P.AddLineNumber(parametersIlGenerator_P);
         weavingInfo_P.AddFilePath(parametersIlGenerator_P);
         weavingInfo_P.AddFileName(parametersIlGenerator_P);
      }

       public void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForInstruction> generator_P)
      {
      }
   }
}
