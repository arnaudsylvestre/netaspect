using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public class MethodWeavingDetector<TMember> : IMethodWeavingDetector where TMember : MemberReference, ICustomAttributeProvider
    {
        public delegate bool IsMethodCompliant(NetAspectDefinition aspect, MethodDefinition method);

        private readonly IsMethodCompliant isMethodCompliant;
        private readonly Func<MethodDefinition, TMember> memberProvider;
        private readonly SelectorProvider<TMember> selectorProvider;
        private readonly IAroundMethodWeaverFactory aroundMethodWeaverFactory;

        public MethodWeavingDetector(IsMethodCompliant isMethodCompliant)
        {
            this.isMethodCompliant = isMethodCompliant;
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
                Befores = aroundMethodWeaverFactory.CreateForBefore(),
                Afters = aroundMethodWeaverFactory.CreateForAfter(),
                OnExceptions = aroundMethodWeaverFactory.CreateForExceptions(),
                Befores = aroundMethodWeaverFactory.CreateForBefore(),
                };
        }
    }

    internal interface IAroundMethodWeaverFactory
    {
    }
}