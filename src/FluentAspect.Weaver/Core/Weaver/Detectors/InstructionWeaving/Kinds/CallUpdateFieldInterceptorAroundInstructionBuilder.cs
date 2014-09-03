using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Kinds
{
   public class CallUpdateFieldInterceptorAroundInstructionBuilder : IInterceptorAroundInstructionBuilder
   {
      public void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations parametersIlGenerator_P)
      {
         weavingInfo_P.AddCalled(parametersIlGenerator_P, weavingInfo_P.GetOperandAsField())
            .AddCalledFieldInfo(parametersIlGenerator_P)
            .AddCallerParameterNames(parametersIlGenerator_P);

         weavingInfo_P.AddColumnNumber(parametersIlGenerator_P)
            .AddLineNumber(parametersIlGenerator_P)
            .AddFilePath(parametersIlGenerator_P)
            .AddFileName(parametersIlGenerator_P)
            .AddCaller(parametersIlGenerator_P)
            .AddCallerParameters(parametersIlGenerator_P)
            .AddCallerMethod(parametersIlGenerator_P)
            ;

         weavingInfo_P.AddFieldValue(parametersIlGenerator_P);
      }

      public void FillBeforeSpecific(InstructionWeavingInfo weavingInfo_P)
      {
      }

      public void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations generator_P)
      {
      }
   }
}
