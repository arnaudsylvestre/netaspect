using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine
{
    public class AroundMethodWeaverFactory : IAroundMethodWeaverFactory
    {
        private IInterceptorAroundMethodBuilder builder;

        public AroundMethodWeaverFactory(IInterceptorAroundMethodBuilder builder)
        {
            this.builder = builder;
        }

        public IIlInjector<IlInjectorAvailableVariables> Create(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect,
            Action<MethodWeavingInfo, InterceptorParameterConfigurations<IlInjectorAvailableVariables>> fillSpecific)
        {
            MethodWeavingInfo weavingInfo_P = new MethodWeavingInfo()
                {
                    Interceptor                    = interceptorMethod,
                    Method                    = method,
                };
            var interceptorParameterConfigurations = new InterceptorParameterConfigurations<IlInjectorAvailableVariables>();
            builder.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
            fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

            return new Injector<IlInjectorAvailableVariables>(method, interceptorMethod, 
                                                                                        aspect, null);
        }

        public IIlInjector<IlInjectorAvailableVariables> CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect)
        {
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillBeforeSpecific(info, interceptorParameterConfigurations));
        }

        public IIlInjector<IlInjectorAvailableVariables> CreateForOnFinally(MethodDefinition method,
                                                                                   MethodInfo interceptorMethod,
                                                                                   NetAspectDefinition aspect)
        {
           return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillOnFinallySpecific(info, interceptorParameterConfigurations));
        }

        private static void FillCommon(MethodDefinition method
                                       )
        {
            //parametersIlGenerator.CreateIlGeneratorForInstanceParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            //parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            //parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        public IIlInjector<IlInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect)
        {
            FillCommon(method);
           // checker.CreateCheckerForResultParameter(method);
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillAfterSpecific(info, interceptorParameterConfigurations));
        }

        public IIlInjector<IlInjectorAvailableVariables> CreateForExceptions(MethodDefinition method,
                                                                                     MethodInfo interceptorMethod,
                                                                                     NetAspectDefinition aspect)
        {
            FillCommon(method);
            //checker.CreateCheckerForExceptionParameter();
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillOnExceptionSpecific(info, interceptorParameterConfigurations));
        }
    }
}