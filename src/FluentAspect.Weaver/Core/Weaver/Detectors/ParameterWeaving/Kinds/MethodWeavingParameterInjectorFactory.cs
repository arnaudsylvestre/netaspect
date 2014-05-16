using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Kinds
{
   public class MethodWeavingParameterInjectorFactory : IInterceptorAroundMethodForParameterBuilder
    {
        public void FillCommon(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
           weavingInfo_P.AddParameterValue(interceptorParameterConfigurations_P)
                         .AddParameterName(interceptorParameterConfigurations_P)
                         .AddParameterInfo(interceptorParameterConfigurations_P)
                         .AddInstance(interceptorParameterConfigurations_P)
                         .AddConstructor(interceptorParameterConfigurations_P)
                         .AddParameters(interceptorParameterConfigurations_P)
                         .AddParameterNames(interceptorParameterConfigurations_P)
                         ;
        }

        public void FillBeforeSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {

        }

        public void FillAfterSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            
        }

        public void FillOnExceptionSpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            weavingInfo_P.AddException(interceptorParameterConfigurations_P);

        }

        public void FillOnFinallySpecific(ParameterWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {

        }
    }
}