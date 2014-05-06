using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker.Peverify;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Kinds;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.AspectCheckers;
using NetAspect.Weaver.Core.Weaver.Engine.AspectFinders;
using NetAspect.Weaver.Core.Weaver.Engine.AssemblyPoolFactories;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver
{
    public static class WeaverFactory
   {
      public static WeaverEngine Create()
      {
         AspectBuilder aspectBuilder = new AspectBuilder(new Dictionary<LifeCycle, ILifeCycleHandler>()
         {
            {LifeCycle.Transient, new TransientLifeCycleHandler()}
         });
         return new WeaverEngine(
            new WeavingModelComputer(new DefaultAspectFinder(),
                     new DefaultAspectChecker(),
                     new List<ICallWeavingDetector>
                     {
                          BuildCallGetFieldDetector(aspectBuilder),
                          BuildCallUpdateFieldDetector(aspectBuilder),
                          BuildCallMethodDetector(aspectBuilder),
                          BuildCallGetPropertyDetector(aspectBuilder),
                          BuildCallUpdatePropertyDetector(aspectBuilder),
                     },
                     new List<IMethodWeavingDetector>
                     {
                             
                     }),
            new DefaultAssemblyPoolFactory(new PeVerifyAssemblyChecker()));
      }

       private static InstructionWeavingDetector<FieldDefinition> BuildCallGetFieldDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsGetFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallGetFieldInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as FieldReference).Resolve(),
              aspect => aspect.BeforeGetField,
              aspect => aspect.AfterGetField);
      }

      private static InstructionWeavingDetector<FieldDefinition> BuildCallUpdateFieldDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<FieldDefinition>(
              InstructionCompliance.IsUpdateFieldInstruction,
              aspect => aspect.FieldSelector,
              new AroundInstructionWeaverFactory(new CallUpdateFieldInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as FieldReference).Resolve(),
              aspect => aspect.BeforeUpdateField,
              aspect => aspect.AfterUpdateField);
      }
      private static InstructionWeavingDetector<PropertyDefinition> BuildCallUpdatePropertyDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsSetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallSetPropertyInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
              aspect => aspect.BeforeSetProperty,
              aspect => aspect.AfterSetProperty);
      }
      private static InstructionWeavingDetector<PropertyDefinition> BuildCallGetPropertyDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<PropertyDefinition>(
              InstructionCompliance.IsGetPropertyCall,
              aspect => aspect.PropertySelector,
              new AroundInstructionWeaverFactory(new CallGetPropertyInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as MethodReference).Resolve().GetPropertyForGetter(),
              aspect => aspect.BeforeGetProperty,
              aspect => aspect.AfterGetProperty);
      }

      private static InstructionWeavingDetector<MethodDefinition> BuildCallMethodDetector(AspectBuilder aspectBuilder)
      {
          return new InstructionWeavingDetector<MethodDefinition>(
              InstructionCompliance.IsCallMethodInstruction,
              aspect => aspect.MethodSelector,
              new AroundInstructionWeaverFactory(new CallMethodInterceptorAroundInstructionBuilder(), aspectBuilder),
              instruction => (instruction.Operand as MethodReference).Resolve(),
              aspect => aspect.BeforeCallMethod,
              aspect => aspect.AfterCallMethod);
      }


   }
}
