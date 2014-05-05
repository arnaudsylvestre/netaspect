using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving
{


    public class InstructionWeavingDetector<TMember> : ICallWeavingDetector where TMember : MemberReference, ICustomAttributeProvider
    {
        public delegate bool IsInstructionCompliant(
            Instruction instruction, NetAspectDefinition aspect, MethodDefinition method);

        private readonly IsInstructionCompliant isInstructionCompliant;
        private readonly SelectorProvider<TMember> selectorProvider;
        private readonly AroundInstructionWeaverFactory aroundInstructionWeaverFactory;
        private readonly Func<Instruction, TMember> memberProvider;

        private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;
        private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;

        public InstructionWeavingDetector(IsInstructionCompliant isInstructionCompliant, SelectorProvider<TMember> selectorProvider, AroundInstructionWeaverFactory aroundInstructionWeaverFactory, Func<Instruction, TMember> memberProvider, Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider, Func<NetAspectDefinition, Interceptor> afterInterceptorProvider)
        {
            this.isInstructionCompliant = isInstructionCompliant;
            this.selectorProvider = selectorProvider;
            this.aroundInstructionWeaverFactory = aroundInstructionWeaverFactory;
            this.memberProvider = memberProvider;
            this.beforeInterceptorProvider = beforeInterceptorProvider;
            this.afterInterceptorProvider = afterInterceptorProvider;
        }


        public IAroundInstructionWeaver DetectWeavingModel(MethodDefinition method, Instruction instruction, NetAspectDefinition aspect)
        {
            if (!isInstructionCompliant(instruction, aspect, method))
                return null;

            var memberReference = memberProvider(instruction);
            if (!AspectApplier.CanApply(memberReference, aspect, selectorProvider))
                return null;

            return new AroundInstructionWeaver(
                aroundInstructionWeaverFactory.CreateForBefore(method, beforeInterceptorProvider(aspect).Method, aspect, instruction),
                aroundInstructionWeaverFactory.CreateForAfter(method, afterInterceptorProvider(aspect).Method, aspect, instruction)
                );
        }
    }
}