using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;

namespace NetAspect.Weaver.Core.Weaver.Instruction.Detector
{
   public class IlInjectorsFactoryForInstruction
   {
      private readonly IInterceptorParameterConfigurationForInstructionFiller _interceptorParameterConfigurationForInstructionFiller;
      private readonly IWeavingPreconditionInjector<VariablesForInstruction> weavingPreconditionInjector;
      private static readonly Action<IInterceptorParameterConfigurationForInstructionFiller, InstructionWeavingInfo, InterceptorParameterPossibilities<VariablesForInstruction>> NoSpecific = (factory, interceptorInfo, generator) => { };

       public IlInjectorsFactoryForInstruction(IInterceptorParameterConfigurationForInstructionFiller interceptorParameterConfigurationForInstructionFiller, IWeavingPreconditionInjector<VariablesForInstruction> weavingPreconditionInjector)
      {
         _interceptorParameterConfigurationForInstructionFiller = interceptorParameterConfigurationForInstructionFiller;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public IIlInjector<VariablesForInstruction> CreateForBefore(MethodDefinition method, Interceptors interceptors, Mono.Cecil.Cil.Instruction instruction)
      {
          return Create(method, interceptors, instruction, NoSpecific);
      }

       private IIlInjector<VariablesForInstruction> Create(MethodDefinition method,
         Interceptors interceptors,
         Mono.Cecil.Cil.Instruction instruction,
         Action<IInterceptorParameterConfigurationForInstructionFiller, InstructionWeavingInfo, InterceptorParameterPossibilities<VariablesForInstruction>> specificFiller)
      {
          if (!interceptors.Methods.Any())
            return new NoIIlInjector<VariablesForInstruction>();

         var info = new InstructionWeavingInfo
         {
            Instruction = instruction,
            Interceptor = new List<MethodInfo>(interceptors.Methods),
            Method = method,
         };
         var parametersIlGenerator = new InterceptorParameterPossibilities<VariablesForInstruction>();
         _interceptorParameterConfigurationForInstructionFiller.FillCommon(info, parametersIlGenerator);
         specificFiller(_interceptorParameterConfigurationForInstructionFiller, info, parametersIlGenerator);

         return new Injector<VariablesForInstruction>(method, info.Interceptor, parametersIlGenerator, weavingPreconditionInjector);
      }


      public IIlInjector<VariablesForInstruction> CreateForAfter(MethodDefinition method,
         Interceptors interceptors,
         Mono.Cecil.Cil.Instruction instruction)
      {
         return Create(method, interceptors, instruction, (factory, interceptorInfo, generator) => factory.FillAfterSpecific(interceptorInfo, generator));
      }
   }
}
