﻿using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public class AroundMethodWeaverFactory
    {
        private IInterceptorAroundMethodBuilder builder;
       private AspectBuilder aspectBuilder;

       public AroundMethodWeaverFactory(IInterceptorAroundMethodBuilder builder, AspectBuilder aspectBuilder_P)
       {
          this.builder = builder;
          aspectBuilder = aspectBuilder_P;
       }

       public IIlInjector Create(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect,
            Action<MethodWeavingInfo, InterceptorParameterConfigurations> fillSpecific)
        {
            MethodWeavingInfo weavingInfo_P = new MethodWeavingInfo()
                {
                    Interceptor                    = interceptorMethod,
                    Method                    = method,
                };
            var interceptorParameterConfigurations = new InterceptorParameterConfigurations();
            builder.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
            fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

            return new Injector(method, interceptorMethod,
                                                                                        aspect, interceptorParameterConfigurations, aspectBuilder);
        }

        public IIlInjector CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect)
        {
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillBeforeSpecific(info, interceptorParameterConfigurations));
        }

        public IIlInjector CreateForOnFinally(MethodDefinition method,
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

        public IIlInjector CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect)
        {
            FillCommon(method);
           // checker.CreateCheckerForResultParameter(method);
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillAfterSpecific(info, interceptorParameterConfigurations));
        }

        public IIlInjector CreateForExceptions(MethodDefinition method,
                                                                                     MethodInfo interceptorMethod,
                                                                                     NetAspectDefinition aspect)
        {
            FillCommon(method);
            //checker.CreateCheckerForExceptionParameter();
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillOnExceptionSpecific(info, interceptorParameterConfigurations));
        }
    }
}