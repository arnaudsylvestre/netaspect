using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using ICustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public class ParameterWeavingDetector : IMethodWeavingDetector
    {
        public delegate bool IsParameterCompliant(NetAspectDefinition aspect, ParameterDefinition parameter, MethodDefinition method);

        private readonly IsParameterCompliant _isParameterCompliant;
        private readonly SelectorProvider<ParameterDefinition> selectorProvider;
        private readonly AroundMethodForParameterWeaverFactory aroundMethodWeaverFactory;


        private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;
        private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;

        private readonly Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider;
        private readonly Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider;

        public ParameterWeavingDetector(Func<NetAspectDefinition, Interceptor> afterInterceptorProvider, AroundMethodForParameterWeaverFactory aroundMethodWeaverFactory, Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider, IsParameterCompliant _isParameterCompliant, Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider, Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider, SelectorProvider<ParameterDefinition> selectorProvider)
        {
            this.afterInterceptorProvider = afterInterceptorProvider;
            this.aroundMethodWeaverFactory = aroundMethodWeaverFactory;
            this.beforeInterceptorProvider = beforeInterceptorProvider;
            this._isParameterCompliant = _isParameterCompliant;
            this.onExceptionInterceptorProvider = onExceptionInterceptorProvider;
            this.onFinallyInterceptorProvider = onFinallyInterceptorProvider;
            this.selectorProvider = selectorProvider;
        }

        public AroundMethodWeavingModel DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect)
        {
           var beforeConstructorBaseCalls = new List<IIlInjector>();
           var afters = new List<IIlInjector>();
           var onExceptions = new List<IIlInjector>();
           var onFinallys = new List<IIlInjector>();
           foreach (var parameter_L in method.Parameters)
           {
              if (!_isParameterCompliant(aspect, parameter_L, method))
                 continue;

              if (!AspectApplier.CanApply(parameter_L, aspect, selectorProvider, method.Module))
                 continue;

              beforeConstructorBaseCalls.Add(aroundMethodWeaverFactory.CreateForBefore(method, beforeInterceptorProvider(aspect).Method, parameter_L));
              afters.Add(aroundMethodWeaverFactory.CreateForAfter(method, afterInterceptorProvider(aspect).Method, parameter_L));
              onExceptions.Add(aroundMethodWeaverFactory.CreateForExceptions(method, onExceptionInterceptorProvider(aspect).Method, parameter_L));
              onFinallys.Add(aroundMethodWeaverFactory.CreateForOnFinally(method, onFinallyInterceptorProvider(aspect).Method, parameter_L));
           }


           var aroundMethodWeavingModel_L = new AroundMethodWeavingModel()
           {
              Afters = afters,
              OnExceptions = onExceptions,
              OnFinallys = onFinallys,
           };
           if (method.IsConstructor)
              aroundMethodWeavingModel_L.BeforeConstructorBaseCalls = beforeConstructorBaseCalls;
           else
              aroundMethodWeavingModel_L.Befores = beforeConstructorBaseCalls;
           return aroundMethodWeavingModel_L;
        }
    }
}