using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Model;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.InstructionWeaving.Engine
{
    public class AroundInstructionWeaverFactory
    {
        private class NoIIlInjector<T> : IIlInjector<T>
        {
            public void Check(ErrorHandler errorHandler)
            {

            }

            public void Inject(List<Instruction> instructions, T availableInformations)
            {

            }
        }

        private readonly IInterceptorAroundInstructionBuilder _interceptorAroundInstructionBuilder;

        public AroundInstructionWeaverFactory(IInterceptorAroundInstructionBuilder interceptorAroundInstructionBuilder)
        {
            _interceptorAroundInstructionBuilder = interceptorAroundInstructionBuilder;
        }

        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction)
        {
            return Create(method, interceptorMethod, aspect, instruction, (factory, interceptorInfo, generator) => factory.FillBeforeSpecific(interceptorInfo));
        }

        private IIlInjector<IlInjectorAvailableVariablesForInstruction> Create(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect,
                                   Instruction instruction, Action<IInterceptorAroundInstructionBuilder, InstructionWeavingInfo, InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction>> specificFiller)
        {
            if (interceptorMethod == null)
                return new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();

            var info = new InstructionWeavingInfo()
                {
                    Instruction = instruction,
                    Interceptor = interceptorMethod,
                    MethodOfInstruction = method,
                };
           var parametersIlGenerator = new InterceptorParameterConfigurations<IlInjectorAvailableVariablesForInstruction>();
           _interceptorAroundInstructionBuilder.FillCommon(info, parametersIlGenerator);
            specificFiller(_interceptorAroundInstructionBuilder, info, parametersIlGenerator);

            return new Injector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, aspect, parametersIlGenerator);
        }


        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method,
                                                                                      MethodInfo interceptorMethod,
                                                                                      NetAspectDefinition aspect, Instruction instruction)
        {
           return Create(method, interceptorMethod, aspect, instruction, (factory, interceptorInfo, generator) => factory.FillAfterSpecific(interceptorInfo, generator));
        }
    }
}