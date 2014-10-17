using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration
{
   public class ConstructorInterceptorParameterConfigurationForMethodFiller : IInterceptorParameterConfigurationForMethodFiller
   {
       public void FillCommon(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         weavingInfo_P.AddInstance(interceptorParameterConfigurations_P)
            .AddConstructor(interceptorParameterConfigurations_P)
            .AddParameters(interceptorParameterConfigurations_P)
            .AddParameterNames(interceptorParameterConfigurations_P)
            .AddLineNumberForMethod(interceptorParameterConfigurations_P)
            .AddColumnNumberForMethod(interceptorParameterConfigurations_P)
            .AddFileNameForMethod(interceptorParameterConfigurations_P)
            .AddFilePathForMethod(interceptorParameterConfigurations_P)
            ;
      }

       public void FillAfterSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
      }

       public void FillOnExceptionSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         weavingInfo_P.AddException(interceptorParameterConfigurations_P);
      }
   }
}
