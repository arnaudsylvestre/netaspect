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
              new AroundInstructionWeaverFactory(new CallGetFieldInterceptorAroundInstructionFactory()),
              instruction => (instruction.Operand as FieldReference).Resolve());
      }

      private static CallWeavingDetector<FieldDefinition> BuildCallUpdateFieldDetector()
      {
          return new CallWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsUpdateFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallUpdateFieldInterceptorAroundInstructionFactory()),
              instruction => (instruction.Operand as FieldReference).Resolve());
      }
      private static CallWeavingDetector<PropertyDefinition> BuildCallUpdatePropertyDetector()
      {
          return new CallWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsSetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallSetPropertyInterceptorAroundInstructionFactory()),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter());
      }
      private static CallWeavingDetector<PropertyDefinition> BuildCallGetPropertyDetector()
      {
          return new CallWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsGetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallGetPropertyInterceptorAroundInstructionFactory()),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter());
      }

      private static CallWeavingDetector<MethodDefinition> BuildCallMethodDetector()
      {
          return new CallWeavingDetector<MethodDefinition>(
              InstructionCompliance.IsCallMethodInstruction,
              aspect => aspect.MethodSelector,
              new AroundInstructionWeaverFactory(new CallMethodInterceptorAroundInstructionFactory()),
              instruction => (instruction.Operand as MethodReference).Resolve());
      }


   }
}
