using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.Parameters.Detector;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration.Parameters
{
   public class MethodParameterInterceptorParameterConfigurationForMethodFiller : IInterceptorParameterConfigurationForParameterFiller
   {
       public void FillCommon(ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         weavingInfo_P.AddParameterValue(interceptorParameterPossibilitiesP)
            .AddParameterName(interceptorParameterPossibilitiesP)
            .AddParameterInfo(interceptorParameterPossibilitiesP)
            .AddConstructor(interceptorParameterPossibilitiesP)
            .AddParameters(interceptorParameterPossibilitiesP)
            .AddParameterNames(interceptorParameterPossibilitiesP)
            .AddInstance(interceptorParameterPossibilitiesP)
            ;
      }

       public void FillOnExceptionSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         weavingInfo_P.AddException(interceptorParameterPossibilitiesP);
      }

   }
}
