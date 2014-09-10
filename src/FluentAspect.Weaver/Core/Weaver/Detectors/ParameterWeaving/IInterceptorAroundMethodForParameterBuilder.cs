using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving
{
   public interface IInterceptorAroundMethodForParameterBuilder
   {
       void FillCommon(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P);
       void FillOnExceptionSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P);
   }
}
