using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration
{
   public class CallGetFieldInterceptorAroundInstructionBuilder : IInterceptorAroundInstructionBuilder
   {
       public void FillCommon(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> parametersIlGenerator_P)
      {
         weavingInfo_P.AddCalled(parametersIlGenerator_P, weavingInfo_P.GetOperandAsField())
            .AddCalledFieldInfo(parametersIlGenerator_P)
            .AddColumnNumber(parametersIlGenerator_P)
            .AddLineNumber(parametersIlGenerator_P)
            .AddFilePath(parametersIlGenerator_P)
            .AddFileName(parametersIlGenerator_P)
            .AddCaller(parametersIlGenerator_P)
            .AddCallerParameters(parametersIlGenerator_P)
            .AddCallerParameterNames(parametersIlGenerator_P)
            .AddCallerMethod(parametersIlGenerator_P);
      }

       public void FillAfterSpecific(InstructionWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForInstruction> generator_P)
      {
         weavingInfo_P.AddResult(generator_P);
      }
   }
}
