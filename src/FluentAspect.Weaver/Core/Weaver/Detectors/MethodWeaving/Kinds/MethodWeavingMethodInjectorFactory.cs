using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Kinds
{
    public class MethodWeavingMethodInjectorFactory : IInterceptorAroundMethodBuilder
    {
        public void FillCommon(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            weavingInfo_P.AddInstance(interceptorParameterConfigurations_P)
                         .AddMethod(interceptorParameterConfigurations_P)
                         .AddParameters(interceptorParameterConfigurations_P)
                         .AddParameterNames(interceptorParameterConfigurations_P)
                         ;
            
            //checker.CreateCheckerForParameterNameParameter(method);



            //parametersIlGenerator.CreateIlGeneratorForResultParameter();

            //parametersIlGenerator.CreateIlGeneratorForExceptionParameter();
        }

        public void FillBeforeSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {

        }

        public void FillAfterSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {

        }

        public void FillOnExceptionSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {

        }

        public void FillOnFinallySpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {

        }
    }
}