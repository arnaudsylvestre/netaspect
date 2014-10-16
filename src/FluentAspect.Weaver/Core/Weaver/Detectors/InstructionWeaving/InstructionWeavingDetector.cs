using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Aspects;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving
{
   public class InstructionWeavingDetector<TMember> : ICallWeavingDetector where TMember : MemberReference, ICustomAttributeProvider
   {
      public delegate bool IsInstructionCompliant(
         Instruction instruction,
         NetAspectDefinition aspect,
         MethodDefinition method);

      private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;
      private readonly AroundInstructionWeaverFactory aroundInstructionWeaverFactory;
       private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;
      private readonly IsInstructionCompliant isInstructionCompliant;
      private readonly Func<Instruction, TMember> memberProvider;
      private readonly SelectorProvider<TMember> selectorProvider;

      public InstructionWeavingDetector(IsInstructionCompliant isInstructionCompliant,
         SelectorProvider<TMember> selectorProvider,
         AroundInstructionWeaverFactory aroundInstructionWeaverFactory,
         Func<Instruction, TMember> memberProvider,
         Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider,
         Func<NetAspectDefinition, Interceptor> afterInterceptorProvider)
      {
         this.isInstructionCompliant = isInstructionCompliant;
         this.selectorProvider = selectorProvider;
         this.aroundInstructionWeaverFactory = aroundInstructionWeaverFactory;
         this.memberProvider = memberProvider;
         this.beforeInterceptorProvider = beforeInterceptorProvider;
         this.afterInterceptorProvider = afterInterceptorProvider;
      }


      public IEnumerable<AroundInstructionWeaver> DetectWeavingModel(MethodDefinition method, Instruction instruction, NetAspectDefinition aspect)
      {
         if (!isInstructionCompliant(instruction, aspect, method))
            return null;

         TMember memberReference = memberProvider(instruction);
         if (!AspectApplier.CanApply(memberReference, aspect, selectorProvider))
            return null;

         var customAttributes = memberReference.GetAspectAttributes(aspect).ToList();
         if (customAttributes.Count == 0)
             return new List<AroundInstructionWeaver>()
                  {
                      CreateAroundInstructionWeaver(method, instruction, aspect, null)
                  };
         return customAttributes.Select(customAttribute => CreateAroundInstructionWeaver(method, instruction, aspect, customAttribute));
      }

       private AroundInstructionWeaver CreateAroundInstructionWeaver(MethodDefinition method, Instruction instruction, NetAspectDefinition aspect, CustomAttribute customAttribute)
       {
           return new AroundInstructionWeaver(customAttribute,
                                              aspect,
                                              aroundInstructionWeaverFactory.CreateForBefore(method, beforeInterceptorProvider(aspect).Method, instruction),
                                              aroundInstructionWeaverFactory.CreateForAfter(method, afterInterceptorProvider(aspect).Method, instruction)
               );
       }
   }
}
