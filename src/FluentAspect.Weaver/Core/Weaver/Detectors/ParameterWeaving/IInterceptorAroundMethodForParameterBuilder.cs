using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving
{
   public interface IInterceptorAroundMethodForParameterBuilder<T>
   {
       void FillCommon(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P);
       void FillBeforeSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P);
       void FillAfterSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P);
       void FillOnExceptionSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P);
       void FillOnFinallySpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P);
   }
}
