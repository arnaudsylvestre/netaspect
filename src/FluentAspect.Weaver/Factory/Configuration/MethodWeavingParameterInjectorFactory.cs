using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving;

namespace NetAspect.Weaver.Factory.Configuration
{
   public class MethodWeavingParameterInjectorFactory : IInterceptorAroundMethodForParameterBuilder
   {
       public void FillCommon(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         weavingInfo_P.AddParameterValue(interceptorParameterConfigurations_P)
            .AddParameterName(interceptorParameterConfigurations_P)
            .AddParameterInfo(interceptorParameterConfigurations_P)
            .AddConstructor(interceptorParameterConfigurations_P)
            .AddParameters(interceptorParameterConfigurations_P)
            .AddParameterNames(interceptorParameterConfigurations_P)
            .AddInstance(interceptorParameterConfigurations_P)
            ;
      }

       public void FillOnExceptionSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         weavingInfo_P.AddException(interceptorParameterConfigurations_P);
      }

   }
}
