﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using ICustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public class MethodWeavingDetector<TMember> : IMethodWeavingDetector where TMember : MemberReference, ICustomAttributeProvider
    {
        public delegate bool IsMethodCompliant(NetAspectDefinition aspect, MethodDefinition method);

        private readonly IsMethodCompliant isMethodCompliant;
        private readonly Func<MethodDefinition, TMember> memberProvider;
        private readonly SelectorProvider<TMember> selectorProvider;
        private readonly IAroundMethodWeaverFactory aroundMethodWeaverFactory;


        private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;
        private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;

        private readonly Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider;
        private readonly Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider;

        public MethodWeavingDetector(IsMethodCompliant isMethodCompliant, Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider, Func<NetAspectDefinition, Interceptor> afterInterceptorProvider, Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider, Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider)
        {
            this.isMethodCompliant = isMethodCompliant;
            this.beforeInterceptorProvider = beforeInterceptorProvider;
            this.afterInterceptorProvider = afterInterceptorProvider;
            this.onExceptionInterceptorProvider = onExceptionInterceptorProvider;
            this.onFinallyInterceptorProvider = onFinallyInterceptorProvider;
        }

        public AroundMethodWeavingModel DetectWeavingModel(MethodDefinition method, NetAspectDefinition aspect)
        {
            if (!isMethodCompliant(aspect, method))
                return null;

            var memberReference = memberProvider(method);
            if (!AspectApplier.CanApply(memberReference, aspect, selectorProvider))
                return null;

            return new AroundMethodWeavingModel()
            {
                Befores = new List<IIlInjector<IlInjectorAvailableVariables>>{aroundMethodWeaverFactory.CreateForBefore(method, beforeInterceptorProvider(aspect).Method, aspect)},
                Afters = new List<IIlInjector<IlInjectorAvailableVariables>> { aroundMethodWeaverFactory.CreateForAfter(method, afterInterceptorProvider(aspect).Method, aspect) },
                OnExceptions = new List<IIlInjector<IlInjectorAvailableVariables>> { aroundMethodWeaverFactory.CreateForExceptions(method, onExceptionInterceptorProvider(aspect).Method, aspect) },
                OnFinallys = new List<IIlInjector<IlInjectorAvailableVariables>> { aroundMethodWeaverFactory.CreateForOnFinally(method, onFinallyInterceptorProvider(aspect).Method, aspect) }
                };
        }
    }

    internal interface IAroundMethodWeaverFactory
    {
        IIlInjector<IlInjectorAvailableVariables> CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect);
        IIlInjector<IlInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect);
        IIlInjector<IlInjectorAvailableVariables> CreateForExceptions(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect);
        IIlInjector<IlInjectorAvailableVariables> CreateForOnFinally(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect);
    }
}