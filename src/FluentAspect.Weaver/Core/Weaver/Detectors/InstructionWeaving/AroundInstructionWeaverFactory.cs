using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ATrier;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving
{
   public class AroundInstructionWeaverFactory
   {
      private readonly IInterceptorAroundInstructionBuilder _interceptorAroundInstructionBuilder;
      private readonly IWevingPreconditionInjector weavingPreconditionInjector;

      public AroundInstructionWeaverFactory(IInterceptorAroundInstructionBuilder interceptorAroundInstructionBuilder, IWevingPreconditionInjector weavingPreconditionInjector)
      {
         _interceptorAroundInstructionBuilder = interceptorAroundInstructionBuilder;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, Instruction instruction)
      {
         return Create(method, interceptorMethod, instruction, (factory, interceptorInfo, generator) => factory.FillBeforeSpecific(interceptorInfo));
      }

      private IIlInjector Create(MethodDefinition method,
         MethodInfo interceptorMethod,
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

         return new Injector(method, interceptorMethod, parametersIlGenerator, weavingPreconditionInjector);
      }


      public IIlInjector CreateForAfter(MethodDefinition method,
         MethodInfo interceptorMethod,
         Instruction instruction)
      {
         return Create(method, interceptorMethod, instruction, (factory, interceptorInfo, generator) => factory.FillAfterSpecific(interceptorInfo, generator));
      }
   }
}
