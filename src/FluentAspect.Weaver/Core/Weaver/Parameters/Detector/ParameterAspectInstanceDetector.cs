using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Session;
using NetAspect.Weaver.Core.Weaver.ToSort.Aspects;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;

namespace NetAspect.Weaver.Core.Weaver.Parameters.Detector
{
   public class ParameterAspectInstanceDetector : IMethodAspectInstanceDetector
   {
      public delegate bool IsParameterCompliant(NetAspectDefinition aspect, ParameterDefinition parameter, MethodDefinition method);

      private readonly IsParameterCompliant _isParameterCompliant;
      private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;
      private readonly IlInjectorsFactoryForParameter _ilInjectorsFactory;


      private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;

      private readonly Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider;
      private readonly Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider;
      private readonly SelectorProvider<ParameterDefinition> selectorProvider;

      public ParameterAspectInstanceDetector(Func<NetAspectDefinition, Interceptor> afterInterceptorProvider,
         IlInjectorsFactoryForParameter _ilInjectorsFactory,
         Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider,
         IsParameterCompliant _isParameterCompliant,
         Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider,
         Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider,
         SelectorProvider<ParameterDefinition> selectorProvider)
      {
         this.afterInterceptorProvider = afterInterceptorProvider;
         this._ilInjectorsFactory = _ilInjectorsFactory;
         this.beforeInterceptorProvider = beforeInterceptorProvider;
         this._isParameterCompliant = _isParameterCompliant;
         this.onExceptionInterceptorProvider = onExceptionInterceptorProvider;
         this.onFinallyInterceptorProvider = onFinallyInterceptorProvider;
         this.selectorProvider = selectorProvider;
      }

      public IEnumerable<AspectInstanceForMethod> GetAspectInstances(MethodDefinition method, NetAspectDefinition aspect)
      {
          var beforeConstructorBaseCalls = new List<IIlInjector<VariablesForMethod>>();
          var afters = new List<IIlInjector<VariablesForMethod>>();
          var onExceptions = new List<IIlInjector<VariablesForMethod>>();
          var onFinallys = new List<IIlInjector<VariablesForMethod>>();
          List<AspectInstanceForMethod> instances = new List<AspectInstanceForMethod>();
         foreach (ParameterDefinition parameter_L in method.Parameters)
         {
            if (!_isParameterCompliant(aspect, parameter_L, method))
               continue;

            if (!AspectApplier.CanApply(parameter_L, aspect, selectorProvider, method.Module))
               continue;

            var customAttributes = parameter_L.GetAspectAttributes(aspect, method.Module).ToList();
            if (customAttributes.Count == 0)
                return new List<AspectInstanceForMethod>()
                  {
                      CreateAspectInstanceForMethodWeaving(method, aspect, null, parameter_L)
                  };
            instances.AddRange(customAttributes.Select(customAttribute => CreateAspectInstanceForMethodWeaving(method, aspect, customAttribute, parameter_L)));
         }


         return instances;
      }

       private AspectInstanceForMethod CreateAspectInstanceForMethodWeaving(MethodDefinition method, NetAspectDefinition aspect, CustomAttribute customAttribute, ParameterDefinition parameter_L)
       {
           return new AspectInstanceForMethod
               {
                   Instance = customAttribute,
                   Aspect = aspect,
                   Befores = new List<IIlInjector<VariablesForMethod>> { _ilInjectorsFactory.CreateForBefore(method, beforeInterceptorProvider(aspect).Method, parameter_L) },
                   Afters = new List<IIlInjector<VariablesForMethod>> { _ilInjectorsFactory.CreateForAfter(method, afterInterceptorProvider(aspect).Method, parameter_L) },
                   OnExceptions = new List<IIlInjector<VariablesForMethod>> { _ilInjectorsFactory.CreateForExceptions(method, onExceptionInterceptorProvider(aspect).Method, parameter_L) },
                   OnFinallys = new List<IIlInjector<VariablesForMethod>> { _ilInjectorsFactory.CreateForOnFinally(method, onFinallyInterceptorProvider(aspect).Method, parameter_L) }
               };
       }
   }
}
