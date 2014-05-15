﻿using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Kinds
{
    public class PropertyGetterWeavingMethodInjectorFactory : IInterceptorAroundMethodBuilder
    {
        public void FillCommon(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            weavingInfo_P.AddInstance(interceptorParameterConfigurations_P)
                         .AddProperty(interceptorParameterConfigurations_P)
                         ;
        }

        public void FillBeforeSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {

        }

        public void FillAfterSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            weavingInfo_P.AddResult(interceptorParameterConfigurations_P);
        }

        public void FillOnExceptionSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            weavingInfo_P.AddException(interceptorParameterConfigurations_P);

        }

        public void FillOnFinallySpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {

        }
    }
}