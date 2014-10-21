using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Instruction.Detector;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;
using NetAspect.Weaver.Factory.Configuration;
using NetAspect.Weaver.Factory.Configuration.Instruction;
using NetAspect.Weaver.Factory.Tools;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Factory
{
    public static class InstructionWeavingDetectorFactory
    {

        public static InstructionAspectInstanceDetector<FieldDefinition> BuildCallGetFieldDetector()
        {
            return new InstructionAspectInstanceDetector<FieldDefinition>(
               InstructionCompliance.IsGetFieldInstruction,
               aspect => aspect.FieldSelector,
               new IlInjectorsFactoryForInstruction(new CallGetFieldInterceptorParameterConfigurationForInstructionFiller(), new NoWeavingPreconditionInjector()),
               instruction => (instruction.Operand as FieldReference).Resolve(),
               aspect => aspect.BeforeGetField,
               aspect => aspect.AfterGetField);
        }

        public static InstructionAspectInstanceDetector<FieldDefinition> BuildCallUpdateFieldDetector()
        {
            return new InstructionAspectInstanceDetector<FieldDefinition>(
               InstructionCompliance.IsUpdateFieldInstruction,
               aspect => aspect.FieldSelector,
               new IlInjectorsFactoryForInstruction(new CallUpdateFieldInterceptorParameterConfigurationForInstructionFiller(), new NoWeavingPreconditionInjector()),
               instruction => (instruction.Operand as FieldReference).Resolve(),
               aspect => aspect.BeforeUpdateField,
               aspect => aspect.AfterUpdateField);
        }

        public static InstructionAspectInstanceDetector<PropertyDefinition> BuildCallUpdatePropertyDetector()
        {
            return new InstructionAspectInstanceDetector<PropertyDefinition>(
               InstructionCompliance.IsSetPropertyCall,
               aspect => aspect.PropertySelector,
               new IlInjectorsFactoryForInstruction(new CallSetPropertyInterceptorParameterConfigurationForInstructionFiller(), new NoWeavingPreconditionInjector()),
               instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForSetter(),
               aspect => aspect.BeforeSetProperty,
               aspect => aspect.AfterSetProperty);
        }

        public static InstructionAspectInstanceDetector<PropertyDefinition> BuildCallGetPropertyDetector()
        {
            return new InstructionAspectInstanceDetector<PropertyDefinition>(
               InstructionCompliance.IsGetPropertyCall,
               aspect => aspect.PropertySelector,
               new IlInjectorsFactoryForInstruction(new CallGetPropertyInterceptorParameterConfigurationForInstructionFiller(), new NoWeavingPreconditionInjector()),
               instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
               aspect => aspect.BeforeGetProperty,
               aspect => aspect.AfterGetProperty);
        }

        public static InstructionAspectInstanceDetector<MethodDefinition> BuildCallMethodDetector()
        {
            return new InstructionAspectInstanceDetector<MethodDefinition>(
               InstructionCompliance.IsCallMethodInstruction,
               aspect => aspect.MethodSelector,
               new IlInjectorsFactoryForInstruction(new CallMethodInterceptorParameterConfigurationForInstructionFiller(), new OverrideWeavingPreconditionInjector()),
               instruction => (instruction.Operand as MethodReference).Resolve(),
               aspect => aspect.BeforeCallMethod,
               aspect => aspect.AfterCallMethod);
        }

        public static InstructionAspectInstanceDetector<MethodDefinition> BuildCallConstructorDetector()
        {
            return new InstructionAspectInstanceDetector<MethodDefinition>(
               InstructionCompliance.IsCallConstructorInstruction,
               aspect => aspect.ConstructorSelector,
               new IlInjectorsFactoryForInstruction(new CallConstructorInterceptorParameterConfigurationForInstructionFiller(), new OverrideWeavingPreconditionInjector()),
               instruction => (instruction.Operand as MethodReference).Resolve(),
               aspect => aspect.BeforeCallConstructor,
               aspect => aspect.AfterCallConstructor);
        }
    }
}