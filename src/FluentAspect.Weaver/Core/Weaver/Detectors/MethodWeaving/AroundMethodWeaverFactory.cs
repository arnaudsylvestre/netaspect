﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
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

       public IIlInjector Create(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Action<MethodWeavingInfo, InterceptorParameterConfigurations> fillSpecific, string interceprotVariableName)
       {
           if (interceptorMethod == null)
               return new NoIIlInjector();

            MethodWeavingInfo weavingInfo_P = new MethodWeavingInfo()
                {
                    Interceptor                    = interceptorMethod,
                    Method                    = method,
                };
            var interceptorParameterConfigurations = new InterceptorParameterConfigurations();
            builder.FillCommon(weavingInfo_P, interceptorParameterConfigurations);
            fillSpecific(weavingInfo_P, interceptorParameterConfigurations);

            return new Injector(method, interceptorMethod, aspect, interceptorParameterConfigurations, aspectBuilder, interceprotVariableName);
        }

        public IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, string interceprotVariableName)
        {
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillBeforeSpecific(info, interceptorParameterConfigurations), interceprotVariableName);
        }

        public IIlInjector CreateForOnFinally(MethodDefinition method,
                                                                                   MethodInfo interceptorMethod,
                                                                                   NetAspectDefinition aspect, string interceptorVariableName)
        {
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillOnFinallySpecific(info, interceptorParameterConfigurations), interceptorVariableName);
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
                                                                               NetAspectDefinition aspect, string interceptorVariableName)
        {
            FillCommon(method);
           // checker.CreateCheckerForResultParameter(method);
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillAfterSpecific(info, interceptorParameterConfigurations), interceptorVariableName);
        }

        public IIlInjector CreateForExceptions(MethodDefinition method,
                                                                                     MethodInfo interceptorMethod,
                                                                                     NetAspectDefinition aspect, string interceptorVariableName)
        {
            FillCommon(method);
            //checker.CreateCheckerForExceptionParameter();
            return Create(method, interceptorMethod, aspect, (info, interceptorParameterConfigurations) => builder.FillOnExceptionSpecific(info, interceptorParameterConfigurations), interceptorVariableName);
        }



        private class NoIIlInjector : IIlInjector
        {
            public void Check(ErrorHandler errorHandler)
            {
            }

            public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
            {
            }
        }

    }
}