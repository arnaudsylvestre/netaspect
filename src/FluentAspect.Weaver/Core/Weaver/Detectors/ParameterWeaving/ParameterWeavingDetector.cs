using System;
using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Aspects;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.ParameterWeaving
{
   public class ParameterWeavingDetector : IMethodWeavingDetector
   {
      public delegate bool IsParameterCompliant(NetAspectDefinition aspect, ParameterDefinition parameter, MethodDefinition method);

      private readonly IsParameterCompliant _isParameterCompliant;
      private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;
      private readonly AroundMethodForParameterWeaverFactory aroundMethodWeaverFactory;


      private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;

      private readonly Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider;
      private readonly Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider;
      private readonly SelectorProvider<ParameterDefinition> selectorProvider;

      public ParameterWeavingDetector(Func<NetAspectDefinition, Interceptor> afterInterceptorProvider,
         AroundMethodForParameterWeaverFactory aroundMethodWeaverFactory,
         Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider,
         IsParameterCompliant _isParameterCompliant,
         Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider,
         Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider,
         SelectorProvider<ParameterDefinition> selectorProvider)
      {
         this.afterInterceptorProvider = afterInterceptorProvider;
         this.aroundMethodWeaverFactory = aroundMethodWeaverFactory;
         this.beforeInterceptorProvider = beforeInterceptorProvider;
         this._isParameterCompliant = _isParameterCompliant;
         this.onExceptionInterceptorProvider = onExceptionInterceptorProvider;
         this.onFinallyInterceptorProvider = onFinallyInterceptorProvider;
         this.selectorProvider = selectorProvider;
      }

      public MethodWeavingAspectInstance DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect)
      {
          var beforeConstructorBaseCalls = new List<IIlInjector<VariablesForMethod>>();
          var afters = new List<IIlInjector<VariablesForMethod>>();
          var onExceptions = new List<IIlInjector<VariablesForMethod>>();
          var onFinallys = new List<IIlInjector<VariablesForMethod>>();
         foreach (ParameterDefinition parameter_L in method.Parameters)
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


         var aroundMethodWeavingModel_L = new MethodWeavingAspectInstance
             {
                 Afters = afters,
                 OnExceptions = onExceptions,
                 OnFinallys = onFinallys,
                 Befores = beforeConstructorBaseCalls,
             };
          return aroundMethodWeavingModel_L;
      }
   }
}
