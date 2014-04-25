using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Called;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Source;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Instance;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Member;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Parameters;
using NetAspect.Weaver.Core.Weaver.Detectors.Helpers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using ICustomAttributeProvider = Mono.Cecil.ICustomAttributeProvider;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{


    public class CallWeavingDetector<TMember> : ICallWeavingDetector where TMember : MemberReference, ICustomAttributeProvider
    {
        public delegate bool IsInstructionCompliant(
            Instruction instruction, NetAspectDefinition aspect, MethodDefinition method);

        private IsInstructionCompliant isInstructionCompliant;
        private SelectorProvider<TMember> selectorProvider;
        private AroundInstructionWeaverFactory aroundInstructionWeaverFactory;
        private Func<Instruction, TMember> memberProvider;

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