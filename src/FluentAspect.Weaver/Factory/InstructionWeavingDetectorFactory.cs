using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving;
using NetAspect.Weaver.Factory.Configuration;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Factory
{
    public static class InstructionWeavingDetectorFactory
    {

        public static InstructionWeavingDetector<FieldDefinition> BuildCallGetFieldDetector()
        {
            return new InstructionWeavingDetector<FieldDefinition>(
               InstructionCompliance.IsGetFieldInstruction,
               aspect => aspect.FieldSelector,
               new AroundInstructionWeaverFactory(new CallGetFieldInterceptorAroundInstructionBuilder(), new NoWeavingPreconditionInjector()),
               instruction => (instruction.Operand as FieldReference).Resolve(),
               aspect => aspect.BeforeGetField,
               aspect => aspect.AfterGetField);
        }

        public static InstructionWeavingDetector<FieldDefinition> BuildCallUpdateFieldDetector()
        {
            return new InstructionWeavingDetector<FieldDefinition>(
               InstructionCompliance.IsUpdateFieldInstruction,
               aspect => aspect.FieldSelector,
               new AroundInstructionWeaverFactory(new CallUpdateFieldInterceptorAroundInstructionBuilder(), new NoWeavingPreconditionInjector()),
               instruction => (instruction.Operand as FieldReference).Resolve(),
               aspect => aspect.BeforeUpdateField,
               aspect => aspect.AfterUpdateField);
        }

        public static InstructionWeavingDetector<PropertyDefinition> BuildCallUpdatePropertyDetector()
        {
            return new InstructionWeavingDetector<PropertyDefinition>(
               InstructionCompliance.IsSetPropertyCall,
               aspect => aspect.PropertySelector,
               new AroundInstructionWeaverFactory(new CallSetPropertyInterceptorAroundInstructionBuilder(), new NoWeavingPreconditionInjector()),
               instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForSetter(),
               aspect => aspect.BeforeSetProperty,
               aspect => aspect.AfterSetProperty);
        }

        public static InstructionWeavingDetector<PropertyDefinition> BuildCallGetPropertyDetector()
        {
            return new InstructionWeavingDetector<PropertyDefinition>(
               InstructionCompliance.IsGetPropertyCall,
               aspect => aspect.PropertySelector,
               new AroundInstructionWeaverFactory(new CallGetPropertyInterceptorAroundInstructionBuilder(), new NoWeavingPreconditionInjector()),
               instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
               aspect => aspect.BeforeGetProperty,
               aspect => aspect.AfterGetProperty);
        }

        public static InstructionWeavingDetector<MethodDefinition> BuildCallMethodDetector()
        {
            return new InstructionWeavingDetector<MethodDefinition>(
               InstructionCompliance.IsCallMethodInstruction,
               aspect => aspect.MethodSelector,
               new AroundInstructionWeaverFactory(new CallMethodInterceptorAroundInstructionBuilder(), new OverrideWeavingPreconditionInjector()),
               instruction => (instruction.Operand as MethodReference).Resolve(),
               aspect => aspect.BeforeCallMethod,
               aspect => aspect.AfterCallMethod);
        }

        public static InstructionWeavingDetector<MethodDefinition> BuildCallConstructorDetector()
        {
            return new InstructionWeavingDetector<MethodDefinition>(
               InstructionCompliance.IsCallConstructorInstruction,
               aspect => aspect.ConstructorSelector,
               new AroundInstructionWeaverFactory(new CallConstructorInterceptorAroundInstructionBuilder(), new OverrideWeavingPreconditionInjector()),
               instruction => (instruction.Operand as MethodReference).Resolve(),
               aspect => aspect.BeforeCallConstructor,
               aspect => aspect.AfterCallConstructor);
        }
    }
}