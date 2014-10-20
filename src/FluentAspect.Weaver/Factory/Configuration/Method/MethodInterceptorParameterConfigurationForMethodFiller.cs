using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.Method.Detector;
using NetAspect.Weaver.Core.Weaver.Method.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration.Method
{
   public class MethodInterceptorParameterConfigurationForMethodFiller : IInterceptorParameterConfigurationForMethodFiller
   {
       public void FillCommon(CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         weavingInfo_P.AddInstance(interceptorParameterPossibilitiesP)
            .AddMethod(interceptorParameterPossibilitiesP)
            .AddParameters(interceptorParameterPossibilitiesP)
            .AddParameterNames(interceptorParameterPossibilitiesP)
            .AddLineNumberForMethod(interceptorParameterPossibilitiesP)
            .AddColumnNumberForMethod(interceptorParameterPossibilitiesP)
            .AddFileNameForMethod(interceptorParameterPossibilitiesP)
            .AddFilePathForMethod(interceptorParameterPossibilitiesP)
            ;
      }

       public void FillAfterSpecific(CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         weavingInfo_P.AddResult(interceptorParameterPossibilitiesP);
      }

       public void FillOnExceptionSpecific(CommonWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         weavingInfo_P.AddException(interceptorParameterPossibilitiesP);
      }
   }
}
