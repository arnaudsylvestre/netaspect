using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker.Peverify;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Field;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Method;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Property;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.AspectCheckers;
using NetAspect.Weaver.Core.Weaver.Engine.AspectFinders;
using NetAspect.Weaver.Core.Weaver.Engine.AssemblyPoolFactories;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver
{
    public static class WeaverFactory
   {
      public static WeaverEngine Create()
      {
         return new WeaverEngine(
            new WeavingModelComputer(new DefaultAspectFinder(),
                     new DefaultAspectChecker(),
                     new List<ICallWeavingDetector>()
                         {
                             BuildCallGetFieldDetector(),
                             BuildCallUpdateFieldDetector(),
                             BuildCallMethodDetector(),
                             BuildCallGetPropertyDetector(),
                             BuildCallUpdatePropertyDetector(),
                         },
                     new List<IMethodWeavingDetector>()
                         {
                             
                         }),
            new DefaultAssemblyPoolFactory(new PeVerifyAssemblyChecker()));
      }

      private static InstructionWeavingDetector<FieldDefinition> BuildCallGetFieldDetector()
      {
          return new InstructionWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsGetFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallGetFieldInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as FieldReference).Resolve(),
              aspect => aspect.BeforeGetField,
              aspect => aspect.AfterGetField);
      }

      private static InstructionWeavingDetector<FieldDefinition> BuildCallUpdateFieldDetector()
      {
          return new InstructionWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsUpdateFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallUpdateFieldInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as FieldReference).Resolve(),
              aspect => aspect.BeforeUpdateField,
              aspect => aspect.AfterUpdateField);
      }
      private static InstructionWeavingDetector<PropertyDefinition> BuildCallUpdatePropertyDetector()
      {
          return new InstructionWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsSetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallSetPropertyInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
              aspect => aspect.BeforeSetProperty,
              aspect => aspect.AfterSetProperty);
      }
      private static InstructionWeavingDetector<PropertyDefinition> BuildCallGetPropertyDetector()
      {
          return new InstructionWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsGetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallGetPropertyInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
              aspect => aspect.BeforeGetProperty,
              aspect => aspect.AfterGetProperty);
      }

      private static InstructionWeavingDetector<MethodDefinition> BuildCallMethodDetector()
      {
          return new InstructionWeavingDetector<MethodDefinition>(
              InstructionCompliance.IsCallMethodInstruction,
              aspect => aspect.MethodSelector,
              new AroundInstructionWeaverFactory(new CallMethodInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as MethodReference).Resolve(),
              aspect => aspect.BeforeCallMethod,
              aspect => aspect.AfterCallMethod);
      }


   }
}
