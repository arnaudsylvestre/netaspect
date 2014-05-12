﻿using System;
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
    public class MethodWeavingDetector<TMember> : IMethodWeavingDetector where TMember : MemberReference, ICustomAttributeProvider
    {
        public delegate bool IsMethodCompliant(NetAspectDefinition aspect, MethodDefinition method);

        private readonly IsMethodCompliant isMethodCompliant;
        private readonly Func<MethodDefinition, TMember> memberProvider;
        private readonly SelectorProvider<TMember> selectorProvider;
        private readonly AroundMethodWeaverFactory aroundMethodWeaverFactory;


        private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;
        private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;

        private readonly Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider;
        private readonly Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider;

        public MethodWeavingDetector(Func<NetAspectDefinition, Interceptor> afterInterceptorProvider, AroundMethodWeaverFactory aroundMethodWeaverFactory, Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider, IsMethodCompliant isMethodCompliant, Func<MethodDefinition, TMember> memberProvider, Func<NetAspectDefinition, Interceptor> onExceptionInterceptorProvider, Func<NetAspectDefinition, Interceptor> onFinallyInterceptorProvider, SelectorProvider<TMember> selectorProvider)
        {
            this.afterInterceptorProvider = afterInterceptorProvider;
            this.aroundMethodWeaverFactory = aroundMethodWeaverFactory;
            this.beforeInterceptorProvider = beforeInterceptorProvider;
            this.isMethodCompliant = isMethodCompliant;
            this.memberProvider = memberProvider;
            this.onExceptionInterceptorProvider = onExceptionInterceptorProvider;
            this.onFinallyInterceptorProvider = onFinallyInterceptorProvider;
            this.selectorProvider = selectorProvider;
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
                Befores = new List<IIlInjector>{aroundMethodWeaverFactory.CreateForBefore(method, beforeInterceptorProvider(aspect).Method, aspect)},
                Afters = new List<IIlInjector> { aroundMethodWeaverFactory.CreateForAfter(method, afterInterceptorProvider(aspect).Method, aspect) },
                OnExceptions = new List<IIlInjector> { aroundMethodWeaverFactory.CreateForExceptions(method, onExceptionInterceptorProvider(aspect).Method, aspect) },
                OnFinallys = new List<IIlInjector> { aroundMethodWeaverFactory.CreateForOnFinally(method, onFinallyInterceptorProvider(aspect).Method, aspect) }
                };
        }
    }
}