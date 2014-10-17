using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Selectors;
using NetAspect.Weaver.Core.Weaver.Aspects;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
   public class MethodAspectInstanceDetector<TMember> : IMethodAspectInstanceDetector where TMember : MemberReference, ICustomAttributeProvider
   {
       public delegate bool IsMethodCompliant(NetAspectDefinition aspect, MethodDefinition method);

      private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;

      private readonly IlInjectorsFactoryForMethod _ilInjectorsFactoryForMethod;


      private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;
      private readonly IsMethodCompliant isMethodCompliant;
      private readonly Func<MethodDefinition, TMember> memberProvider;

      private readonly Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider;
      private readonly Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider;
      private readonly SelectorProvider<TMember> selectorProvider;

      public MethodAspectInstanceDetector(Func<NetAspectDefinition, Interceptor> afterInterceptorProvider,
         IlInjectorsFactoryForMethod _ilInjectorsFactoryForMethod,
         Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider,
         IsMethodCompliant isMethodCompliant,
         Func<MethodDefinition, TMember> memberProvider,
         Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider,
         Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider,
         SelectorProvider<TMember> selectorProvider)
      {
         this.afterInterceptorProvider = afterInterceptorProvider;
         this._ilInjectorsFactoryForMethod = _ilInjectorsFactoryForMethod;
         this.beforeInterceptorProvider = beforeInterceptorProvider;
         this.isMethodCompliant = isMethodCompliant;
         this.memberProvider = memberProvider;
         this.onExceptionInterceptorProvider = onExceptionInterceptorProvider;
         this.onFinallyInterceptorProvider = onFinallyInterceptorProvider;
         this.selectorProvider = selectorProvider;
      }

      public IEnumerable<AspectInstanceForMethodWeaving> GetAspectInstances(MethodDefinition method, NetAspectDefinition aspect)
      {
         if (!isMethodCompliant(aspect, method))
            return null;

         TMember memberReference = memberProvider(method);
         if (!AspectApplier.CanApply(memberReference, aspect, selectorProvider))
            return null;

          var customAttributes = memberReference.GetAspectAttributes(aspect).ToList();
          if (customAttributes.Count == 0)
              return new List<AspectInstanceForMethodWeaving>()
                  {
                      CreateAspectInstanceForMethodWeaving(method, aspect, null)
                  };
          return customAttributes.Select(customAttribute => CreateAspectInstanceForMethodWeaving(method, aspect, customAttribute));
      }

       private AspectInstanceForMethodWeaving CreateAspectInstanceForMethodWeaving(MethodDefinition method, NetAspectDefinition aspect, CustomAttribute customAttribute)
       {
           return new AspectInstanceForMethodWeaving
               {
                   Instance = customAttribute,
                   Aspect = aspect,
                   Befores = new List<IIlInjector<VariablesForMethod>> {_ilInjectorsFactoryForMethod.CreateForBefore(method, beforeInterceptorProvider(aspect).Method)},
                   Afters = new List<IIlInjector<VariablesForMethod>> {_ilInjectorsFactoryForMethod.CreateForAfter(method, afterInterceptorProvider(aspect).Method)}, 
                   OnExceptions = new List<IIlInjector<VariablesForMethod>> {_ilInjectorsFactoryForMethod.CreateForExceptions(method, onExceptionInterceptorProvider(aspect).Method)},
                   OnFinallys = new List<IIlInjector<VariablesForMethod>> {_ilInjectorsFactoryForMethod.CreateForOnFinally(method, onFinallyInterceptorProvider(aspect).Method)}
               };
       }
   }
}
