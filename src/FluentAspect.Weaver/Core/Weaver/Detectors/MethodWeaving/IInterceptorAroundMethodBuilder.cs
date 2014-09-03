using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
   public interface IInterceptorAroundMethodBuilder
   {
      void FillCommon(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
      void FillBeforeSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
      void FillAfterSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
      void FillOnExceptionSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
      void FillOnFinallySpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P);
   }
}
