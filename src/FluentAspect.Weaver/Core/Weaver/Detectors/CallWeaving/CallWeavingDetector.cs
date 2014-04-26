using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using ICustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{


    public class CallWeavingDetector<TMember> : ICallWeavingDetector where TMember : MemberReference, ICustomAttributeProvider
    {
        public delegate bool IsInstructionCompliant(
            Instruction instruction, NetAspectDefinition aspect, MethodDefinition method);

        private readonly IsInstructionCompliant isInstructionCompliant;
        private readonly SelectorProvider<TMember> selectorProvider;
        private readonly AroundInstructionWeaverFactory aroundInstructionWeaverFactory;
        private readonly Func<Instruction, TMember> memberProvider;

        public CallWeavingDetector(IsInstructionCompliant isInstructionCompliant, SelectorProvider<TMember> selectorProvider, AroundInstructionWeaverFactory aroundInstructionWeaverFactory, Func<Instruction, TMember> memberProvider)
        {
            this.isInstructionCompliant = isInstructionCompliant;
            this.selectorProvider = selectorProvider;
            this.aroundInstructionWeaverFactory = aroundInstructionWeaverFactory;
            this.memberProvider = memberProvider;
        }


        public IAroundInstructionWeaver DetectWeavingModel(MethodDefinition method, Instruction instruction, NetAspectDefinition aspect)
        {
            if (!isInstructionCompliant(instruction, aspect, method))
                return null;

            var memberReference = memberProvider(instruction);
            if (!AspectApplier.CanApply(memberReference, aspect, selectorProvider))
                return null;

            return new AroundInstructionWeaver(
                aroundInstructionWeaverFactory.CreateForBefore(method, aspect.BeforeGetField.Method, aspect, instruction),
                aroundInstructionWeaverFactory.CreateForAfter(method, aspect.AfterGetField.Method, aspect, instruction)
                );
        }
    }
}