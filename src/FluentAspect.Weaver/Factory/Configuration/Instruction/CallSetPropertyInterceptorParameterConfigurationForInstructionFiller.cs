using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration
{
   public class CallSetPropertyInterceptorParameterConfigurationForInstructionFiller : IInterceptorParameterConfigurationForInstructionFiller
   {
       public void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> parametersIlGenerator_P)
      {
         weavingInfo_P.AddCalled(parametersIlGenerator_P, weavingInfo_P.GetOperandAsMethod());
         weavingInfo_P.AddCalledPropertyInfo(parametersIlGenerator_P);

         weavingInfo_P.AddCaller(parametersIlGenerator_P);
         weavingInfo_P.AddCallerParameters(parametersIlGenerator_P);
         weavingInfo_P.AddCallerParameterNames(parametersIlGenerator_P);
         weavingInfo_P.AddCallerMethod(parametersIlGenerator_P);

         weavingInfo_P.AddColumnNumber(parametersIlGenerator_P);
         weavingInfo_P.AddLineNumber(parametersIlGenerator_P);
         weavingInfo_P.AddFilePath(parametersIlGenerator_P);
         weavingInfo_P.AddFileName(parametersIlGenerator_P);


         weavingInfo_P.AddPropertyValueForInstruction(parametersIlGenerator_P);
      }

       public void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> generator_P)
      {
      }
   }
}
