using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker.Peverify;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Method;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Property;
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

      private static CallWeavingDetector<FieldDefinition> BuildCallGetFieldDetector()
      {
          return new CallWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsGetFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallGetFieldInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as FieldReference).Resolve(),
              aspect => aspect.BeforeGetField,
              aspect => aspect.AfterGetField);
      }

      private static CallWeavingDetector<FieldDefinition> BuildCallUpdateFieldDetector()
      {
          return new CallWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsUpdateFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallUpdateFieldInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as FieldReference).Resolve(),
              aspect => aspect.BeforeUpdateField,
              aspect => aspect.AfterUpdateField);
      }
      private static CallWeavingDetector<PropertyDefinition> BuildCallUpdatePropertyDetector()
      {
          return new CallWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsSetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallSetPropertyInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
              aspect => aspect.BeforeSetProperty,
              aspect => aspect.AfterSetProperty);
      }
      private static CallWeavingDetector<PropertyDefinition> BuildCallGetPropertyDetector()
      {
          return new CallWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsGetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallGetPropertyInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
              aspect => aspect.BeforeGetProperty,
              aspect => aspect.AfterGetProperty);
      }

      private static CallWeavingDetector<MethodDefinition> BuildCallMethodDetector()
      {
          return new CallWeavingDetector<MethodDefinition>(
              InstructionCompliance.IsCallMethodInstruction,
              aspect => aspect.MethodSelector,
              new AroundInstructionWeaverFactory(new CallMethodInterceptorAroundInstructionBuilder()),
              instruction => (instruction.Operand as MethodReference).Resolve(),
              aspect => aspect.BeforeCallMethod,
              aspect => aspect.AfterCallMethod);
      }


   }
}
