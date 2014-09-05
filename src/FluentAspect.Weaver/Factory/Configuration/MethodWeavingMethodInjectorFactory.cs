using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Factory.Configuration
{
   public class MethodWeavingMethodInjectorFactory : IInterceptorAroundMethodBuilder
   {
       public void FillCommon(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         weavingInfo_P.AddInstance(interceptorParameterConfigurations_P)
            .AddMethod(interceptorParameterConfigurations_P)
            .AddParameters(interceptorParameterConfigurations_P)
            .AddParameterNames(interceptorParameterConfigurations_P)
            .AddLineNumberForMethod(interceptorParameterConfigurations_P)
            .AddFileNameForMethod(interceptorParameterConfigurations_P)
            .AddFilePathForMethod(interceptorParameterConfigurations_P)
            ;
      }

       public void FillBeforeSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
      }

       public void FillAfterSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         weavingInfo_P.AddResult(interceptorParameterConfigurations_P);
      }

       public void FillOnExceptionSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
         weavingInfo_P.AddException(interceptorParameterConfigurations_P);
      }

       public void FillOnFinallySpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<VariablesForMethod> interceptorParameterConfigurations_P)
      {
      }
   }
}
