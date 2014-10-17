using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving
{
   public class IlInjectorsFactoryForInstruction
   {
      private readonly IInterceptorParameterConfigurationForInstructionFiller _interceptorParameterConfigurationForInstructionFiller;
      private readonly IWevingPreconditionInjector<VariablesForInstruction> weavingPreconditionInjector;
      private static readonly Action<IInterceptorParameterConfigurationForInstructionFiller, InstructionWeavingInfo, InterceptorParameterConfigurations<VariablesForInstruction>> NoSpecific = (factory, interceptorInfo, generator) => { };

       public IlInjectorsFactoryForInstruction(IInterceptorParameterConfigurationForInstructionFiller interceptorParameterConfigurationForInstructionFiller, IWevingPreconditionInjector<VariablesForInstruction> weavingPreconditionInjector)
      {
         _interceptorParameterConfigurationForInstructionFiller = interceptorParameterConfigurationForInstructionFiller;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector<VariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, Instruction instruction)
      {
          return Create(method, interceptorMethod, instruction, NoSpecific);
      }

       private IIlInjector<VariablesForInstruction> Create(MethodDefinition method,
         MethodInfo interceptorMethod,
         Instruction instruction,
         Action<IInterceptorParameterConfigurationForInstructionFiller, InstructionWeavingInfo, InterceptorParameterConfigurations<VariablesForInstruction>> specificFiller)
      {
         if (interceptorMethod == null)
            return new NoIIlInjector<VariablesForInstruction>();

         var info = new InstructionWeavingInfo
         {
            Instruction = instruction,
            Interceptor = interceptorMethod,
            Method = method,
         };
         var parametersIlGenerator = new InterceptorParameterConfigurations<VariablesForInstruction>();
         _interceptorParameterConfigurationForInstructionFiller.FillCommon(info, parametersIlGenerator);
         specificFiller(_interceptorParameterConfigurationForInstructionFiller, info, parametersIlGenerator);

         return new Injector<VariablesForInstruction>(method, interceptorMethod, parametersIlGenerator, weavingPreconditionInjector);
      }


      public IIlInjector<VariablesForInstruction> CreateForAfter(MethodDefinition method,
         MethodInfo interceptorMethod,
         Instruction instruction)
      {
         return Create(method, interceptorMethod, instruction, (factory, interceptorInfo, generator) => factory.FillAfterSpecific(interceptorInfo, generator));
      }
   }
}
