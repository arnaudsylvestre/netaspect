using System.Collections.Generic;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Method
{
    public class MethodWeavingMethodInjectorFactory : IInterceptorAroundMethodBuilder
    {
        public void FillCommon(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            //weavingInfo_P.AddInstance()
            //    .AddCurrentMethod()


            //checker.CreateCheckerForMethodParameter();
            //checker.CreateCheckerForParameterNameParameter(method);
            //checker.CreateCheckerForParametersParameter();



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