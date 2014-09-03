using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration
{
   public class CallMethodInterceptorAroundInstructionBuilder : IInterceptorAroundInstructionBuilder
   {
      public void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations parametersIlGenerator_P)
      {
         weavingInfo_P.AddCalled(parametersIlGenerator_P, weavingInfo_P.GetOperandAsMethod());
         weavingInfo_P.AddCalledParameters(parametersIlGenerator_P);
         weavingInfo_P.AddCalledParameterNames(parametersIlGenerator_P);

         weavingInfo_P.AddCaller(parametersIlGenerator_P);
         weavingInfo_P.AddCallerParameters(parametersIlGenerator_P);
         weavingInfo_P.AddCallerParameterNames(parametersIlGenerator_P);
         weavingInfo_P.AddCallerMethod(parametersIlGenerator_P);

         weavingInfo_P.AddColumnNumber(parametersIlGenerator_P);
         weavingInfo_P.AddLineNumber(parametersIlGenerator_P);
         weavingInfo_P.AddFilePath(parametersIlGenerator_P);
         weavingInfo_P.AddFileName(parametersIlGenerator_P);
      }

      public void FillBeforeSpecific()
      {
      }

      public void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations generator_P)
      {
         weavingInfo_P.AddResult(generator_P);
      }
   }
}
