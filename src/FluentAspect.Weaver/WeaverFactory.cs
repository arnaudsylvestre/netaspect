using System.Collections.Generic;
using Mono.Cecil;
using NetAspect.Weaver.Apis.AssemblyChecker.Peverify;
using NetAspect.Weaver.Core.Weaver;
using NetAspect.Weaver.Core.Weaver.Detectors;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.AspectCheckers;
using NetAspect.Weaver.Core.Weaver.Engine.AspectFinders;
using NetAspect.Weaver.Core.Weaver.Engine.AssemblyPoolFactories;

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
                             BuildCallGetFieldDetector()
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
   }
}
