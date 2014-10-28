using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.Instruction.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.Method.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.Parameters.Detector;
using NetAspect.Weaver.Core.Weaver.Parameters.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration.Parameters
{
   public class MethodParameterInterceptorParameterConfigurationForConstructorFiller : IInterceptorParameterConfigurationForParameterFiller
   {
       public void FillCommon(ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilities)
      {
         weavingInfo_P.AddParameterValue(interceptorParameterPossibilities)
            .AddParameterInfo(interceptorParameterPossibilities)
            .AddConstructor(interceptorParameterPossibilities)
            .AddParameters(interceptorParameterPossibilities)
            .AddParameterNames(interceptorParameterPossibilities)
            .AddInstance(interceptorParameterPossibilities)
            ;
      }

       public void FillOnExceptionSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterPossibilities<VariablesForMethod> interceptorParameterPossibilitiesP)
      {
         weavingInfo_P.AddException(interceptorParameterPossibilitiesP);
      }

   }
}
