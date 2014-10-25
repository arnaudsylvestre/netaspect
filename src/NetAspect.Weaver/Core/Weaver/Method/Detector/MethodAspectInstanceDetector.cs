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

namespace NetAspect.Weaver.Core.Weaver.Method.Detector
{
   public class MethodAspectInstanceDetector<TMember> : IMethodAspectInstanceDetector where TMember : MemberReference, ICustomAttributeProvider
   {
       public delegate bool IsMethodCompliant(NetAspectDefinition aspect, MethodDefinition method);

      private readonly Func<NetAspectDefinition, Interceptors> afterInterceptorProvider;

      private readonly IlInjectorsFactoryForMethod _ilInjectorsFactoryForMethod;


      private readonly Func<NetAspectDefinition, Interceptors> beforeInterceptorProvider;
      private readonly IsMethodCompliant isMethodCompliant;
      private readonly Func<MethodDefinition, TMember> memberProvider;

      private readonly Func<NetAspectDefinition, Interceptors> onExceptionInterceptorProvider;
      private readonly Func<NetAspectDefinition, Interceptors> onFinallyInterceptorProvider;
      private readonly SelectorProvider<TMember> selectorProvider;

      public MethodAspectInstanceDetector(Func<NetAspectDefinition, Interceptors> afterInterceptorProvider,
         IlInjectorsFactoryForMethod _ilInjectorsFactoryForMethod,
         Func<NetAspectDefinition, Interceptors> beforeInterceptorProvider,
         IsMethodCompliant isMethodCompliant,
         Func<MethodDefinition, TMember> memberProvider,
         Func<NetAspectDefinition, Interceptors> onExceptionInterceptorProvider,
         Func<NetAspectDefinition, Interceptors> onFinallyInterceptorProvider,
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

      public IEnumerable<AspectInstanceForMethod> GetAspectInstances(MethodDefinition method, NetAspectDefinition aspect)
      {
         if (!isMethodCompliant(aspect, method))
            return null;

         TMember memberReference = memberProvider(method);
         if (!AspectApplier.CanApply(memberReference, aspect, selectorProvider))
            return null;

          var customAttributes = memberReference.GetAspectAttributes(aspect).ToList();
          if (customAttributes.Count == 0)
              return new List<AspectInstanceForMethod>()
                  {
                      CreateAspectInstanceForMethodWeaving(method, aspect, null)
                  };
          return customAttributes.Select(customAttribute => CreateAspectInstanceForMethodWeaving(method, aspect, customAttribute));
      }

       private AspectInstanceForMethod CreateAspectInstanceForMethodWeaving(MethodDefinition method, NetAspectDefinition aspect, CustomAttribute customAttribute)
       {
           return new AspectInstanceForMethod
               {
                   Instance = customAttribute,
                   Aspect = aspect,
                   Befores = new List<IIlInjector<VariablesForMethod>> {_ilInjectorsFactoryForMethod.CreateForBefore(method, beforeInterceptorProvider(aspect))},
                   Afters = new List<IIlInjector<VariablesForMethod>> {_ilInjectorsFactoryForMethod.CreateForAfter(method, afterInterceptorProvider(aspect))}, 
                   OnExceptions = new List<IIlInjector<VariablesForMethod>> {_ilInjectorsFactoryForMethod.CreateForExceptions(method, onExceptionInterceptorProvider(aspect))},
                   OnFinallys = new List<IIlInjector<VariablesForMethod>> {_ilInjectorsFactoryForMethod.CreateForOnFinally(method, onFinallyInterceptorProvider(aspect))}
               };
       }
   }
}
