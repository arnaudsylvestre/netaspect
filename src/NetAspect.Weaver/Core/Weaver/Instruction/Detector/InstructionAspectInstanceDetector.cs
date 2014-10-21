using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Session;
using NetAspect.Weaver.Core.Weaver.ToSort.Aspects;

namespace NetAspect.Weaver.Core.Weaver.Instruction.Detector
{
   public class InstructionAspectInstanceDetector<TMember> : IInstructionAspectInstanceDetector where TMember : MemberReference, ICustomAttributeProvider
   {
      public delegate bool IsInstructionCompliant(
         Mono.Cecil.Cil.Instruction instruction,
         NetAspectDefinition aspect,
         MethodDefinition method);

      private readonly Func<NetAspectDefinition, Interceptor> afterInterceptorProvider;
      private readonly IlInjectorsFactoryForInstruction _ilInjectorsFactoryForInstruction;
       private readonly Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider;
      private readonly IsInstructionCompliant isInstructionCompliant;
      private readonly Func<Mono.Cecil.Cil.Instruction, TMember> memberProvider;
      private readonly SelectorProvider<TMember> selectorProvider;

      public InstructionAspectInstanceDetector(IsInstructionCompliant isInstructionCompliant,
         SelectorProvider<TMember> selectorProvider,
         IlInjectorsFactoryForInstruction _ilInjectorsFactoryForInstruction,
         Func<Mono.Cecil.Cil.Instruction, TMember> memberProvider,
         Func<NetAspectDefinition, Interceptor> beforeInterceptorProvider,
         Func<NetAspectDefinition, Interceptor> afterInterceptorProvider)
      {
         this.isInstructionCompliant = isInstructionCompliant;
         this.selectorProvider = selectorProvider;
         this._ilInjectorsFactoryForInstruction = _ilInjectorsFactoryForInstruction;
         this.memberProvider = memberProvider;
         this.beforeInterceptorProvider = beforeInterceptorProvider;
         this.afterInterceptorProvider = afterInterceptorProvider;
      }


      public IEnumerable<AspectInstanceForInstruction> GetAspectInstances(MethodDefinition method, Mono.Cecil.Cil.Instruction instruction, NetAspectDefinition aspect)
      {
         if (!isInstructionCompliant(instruction, aspect, method))
            return null;

         TMember memberReference = memberProvider(instruction);
         if (!AspectApplier.CanApply(memberReference, aspect, selectorProvider))
            return null;

         var customAttributes = memberReference.GetAspectAttributes(aspect).ToList();
         if (customAttributes.Count == 0)
             return new List<AspectInstanceForInstruction>()
                  {
                      CreateAroundInstructionWeaver(method, instruction, aspect, null)
                  };
         return customAttributes.Select(customAttribute => CreateAroundInstructionWeaver(method, instruction, aspect, customAttribute));
      }

       private AspectInstanceForInstruction CreateAroundInstructionWeaver(MethodDefinition method, Mono.Cecil.Cil.Instruction instruction, NetAspectDefinition aspect, CustomAttribute customAttribute)
       {
           return new AspectInstanceForInstruction()
               {
                   Instance = customAttribute,
                   Aspect = aspect,
                   Before = _ilInjectorsFactoryForInstruction.CreateForBefore(method, beforeInterceptorProvider(aspect).Method, instruction),
                   After = _ilInjectorsFactoryForInstruction.CreateForAfter(method, afterInterceptorProvider(aspect).Method, instruction)
               };
       }
   }
}
