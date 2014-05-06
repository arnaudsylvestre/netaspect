using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving
{
   public class AroundInstructionWeaverFactory
   {
      private readonly IInterceptorAroundInstructionBuilder _interceptorAroundInstructionBuilder;
      private AspectBuilder aspectBuilder;

      public AroundInstructionWeaverFactory(IInterceptorAroundInstructionBuilder interceptorAroundInstructionBuilder, AspectBuilder aspectBuilder_P)
      {
         _interceptorAroundInstructionBuilder = interceptorAroundInstructionBuilder;
         aspectBuilder = aspectBuilder_P;
      }

      public IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction)
      {
         return Create(method, interceptorMethod, aspect, instruction, (factory, interceptorInfo, generator) => factory.FillBeforeSpecific(interceptorInfo));
      }

      private IIlInjector Create(MethodDefinition method,
         MethodInfo interceptorMethod,
         NetAspectDefinition aspect,
         Instruction instruction,
         Action<IInterceptorAroundInstructionBuilder, InstructionWeavingInfo, InterceptorParameterConfigurations> specificFiller)
      {
         if (interceptorMethod == null)
            return new NoIIlInjector();

         var info = new InstructionWeavingInfo
         {
            Instruction = instruction,
            Interceptor = interceptorMethod,
            Method = method,
         };
         var parametersIlGenerator = new InterceptorParameterConfigurations();
         _interceptorAroundInstructionBuilder.FillCommon(info, parametersIlGenerator);
         specificFiller(_interceptorAroundInstructionBuilder, info, parametersIlGenerator);

         return new Injector(method, interceptorMethod, aspect, parametersIlGenerator, aspectBuilder);
      }


      public IIlInjector CreateForAfter(MethodDefinition method,
         MethodInfo interceptorMethod,
         NetAspectDefinition aspect,
         Instruction instruction)
      {
         return Create(method, interceptorMethod, aspect, instruction, (factory, interceptorInfo, generator) => factory.FillAfterSpecific(interceptorInfo, generator));
      }

      private class NoIIlInjector : IIlInjector
      {
         public void Check(ErrorHandler errorHandler)
         {
         }

         public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
         {
         }
      }
   }
}
