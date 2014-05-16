using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public class AroundMethodWeaverFactory
    {
        private IInterceptorAroundMethodBuilder builder;

        public AroundMethodWeaverFactory(IInterceptorAroundMethodBuilder builder)
       {
          this.builder = builder;
       }

       public IIlInjector Create(MethodDefinition method, MethodInfo interceptorMethod, Action<MethodWeavingInfo, InterceptorParameterConfigurations> fillSpecific)
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

            return new Injector(method, interceptorMethod, interceptorParameterConfigurations);
        }

        public IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod)
        {
            return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillBeforeSpecific(info, interceptorParameterConfigurations));
        }

        public IIlInjector CreateForOnFinally(MethodDefinition method,
                                                                                   MethodInfo interceptorMethod)
        {
            return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillOnFinallySpecific(info, interceptorParameterConfigurations));
        }

        public IIlInjector CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod)
        {
            return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillAfterSpecific(info, interceptorParameterConfigurations));
        }

        public IIlInjector CreateForExceptions(MethodDefinition method, MethodInfo interceptorMethod)
        {
            return Create(method, interceptorMethod, (info, interceptorParameterConfigurations) => builder.FillOnExceptionSpecific(info, interceptorParameterConfigurations));
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