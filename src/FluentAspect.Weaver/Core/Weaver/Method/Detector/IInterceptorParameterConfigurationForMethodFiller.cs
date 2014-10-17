using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
   public interface IInterceptorParameterConfigurationForMethodFiller
   {
      void FillCommon(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P);
       void FillAfterSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P);
      void FillOnExceptionSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P);
   }
}
