using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving
{
   public interface IInterceptorAroundMethodForParameterBuilder
   {
      void FillCommon(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
      void FillBeforeSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
      void FillAfterSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
      void FillOnExceptionSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
      void FillOnFinallySpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
   }
}
