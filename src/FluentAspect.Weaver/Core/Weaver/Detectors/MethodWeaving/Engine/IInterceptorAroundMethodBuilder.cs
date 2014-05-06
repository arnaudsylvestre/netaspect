using System.Collections.Generic;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine
{
    public interface IInterceptorAroundMethodBuilder
    {
       void FillCommon(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariables> interceptorParameterConfigurations_P);
        void FillBeforeSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariables> interceptorParameterConfigurations_P);
        void FillAfterSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariables> interceptorParameterConfigurations_P);
        void FillOnExceptionSpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariables> interceptorParameterConfigurations_P);
        void FillOnFinallySpecific(MethodWeavingInfo weavingInfo_P, InterceptorParameterConfigurations<IlInjectorAvailableVariables> interceptorParameterConfigurations_P);
    }
}