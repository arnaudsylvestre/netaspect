using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;

namespace NetAspect.Weaver.Core.Weaver.Instruction.Detector
{
   public class IlInjectorsFactoryForInstruction
   {
      private readonly IInterceptorParameterConfigurationForInstructionFiller _interceptorParameterConfigurationForInstructionFiller;
      private readonly IWevingPreconditionInjector<VariablesForInstruction> weavingPreconditionInjector;
      private static readonly Action<IInterceptorParameterConfigurationForInstructionFiller, InstructionWeavingInfo, InterceptorParameterPossibilities<VariablesForInstruction>> NoSpecific = (factory, interceptorInfo, generator) => { };

       public IlInjectorsFactoryForInstruction(IInterceptorParameterConfigurationForInstructionFiller interceptorParameterConfigurationForInstructionFiller, IWevingPreconditionInjector<VariablesForInstruction> weavingPreconditionInjector)
      {
         _interceptorParameterConfigurationForInstructionFiller = interceptorParameterConfigurationForInstructionFiller;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector<VariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, Mono.Cecil.Cil.Instruction instruction)
      {
          return Create(method, interceptorMethod, instruction, NoSpecific);
      }

       private IIlInjector<VariablesForInstruction> Create(MethodDefinition method,
         MethodInfo interceptorMethod,
         Mono.Cecil.Cil.Instruction instruction,
         Action<IInterceptorParameterConfigurationForInstructionFiller, InstructionWeavingInfo, InterceptorParameterPossibilities<VariablesForInstruction>> specificFiller)
      {
         if (interceptorMethod == null)
            return new NoIIlInjector<VariablesForInstruction>();

         var info = new InstructionWeavingInfo
         {
            Instruction = instruction,
            Interceptor = interceptorMethod,
            Method = method,
         };
         var parametersIlGenerator = new InterceptorParameterPossibilities<VariablesForInstruction>();
         _interceptorParameterConfigurationForInstructionFiller.FillCommon(info, parametersIlGenerator);
         specificFiller(_interceptorParameterConfigurationForInstructionFiller, info, parametersIlGenerator);

         return new Injector<VariablesForInstruction>(method, interceptorMethod, parametersIlGenerator, weavingPreconditionInjector);
      }


      public IIlInjector<VariablesForInstruction> CreateForAfter(MethodDefinition method,
         MethodInfo interceptorMethod,
         Mono.Cecil.Cil.Instruction instruction)
      {
         return Create(method, interceptorMethod, instruction, (factory, interceptorInfo, generator) => factory.FillAfterSpecific(interceptorInfo, generator));
      }
   }
}
