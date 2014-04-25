using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
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

        private readonly IInterceptorAroundInstructionFactory _interceptorAroundInstructionFactory;

        public AroundInstructionWeaverFactory(IInterceptorAroundInstructionFactory interceptorAroundInstructionFactory)
        {
            _interceptorAroundInstructionFactory = interceptorAroundInstructionFactory;
        }

        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction)
        {
            return Create(method, interceptorMethod, aspect, instruction, (factory, interceptorInfo) => factory.FillBeforeSpecific(interceptorInfo));
        }

        private IIlInjector<IlInjectorAvailableVariablesForInstruction> Create(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect,
                                   Instruction instruction, Action<IInterceptorAroundInstructionFactory, InterceptorInfo> specificFiller)
        {
            if (interceptorMethod == null)
                return new NoIIlInjector<IlInjectorAvailableVariablesForInstruction>();

            var checker = new ParametersChecker();
            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            var info = new InterceptorInfo()
                {
                    Generator = parametersIlGenerator,
                    Instruction = instruction,
                    Interceptor = interceptorMethod,
                    Method = method,
                };
            _interceptorAroundInstructionFactory.FillCommon(info);
            specificFiller(_interceptorAroundInstructionFactory, info);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod,
                                                                                                     checker,
                                                                                                     parametersIlGenerator,
                                                                                                     aspect);
        }


        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method,
                                                                                      MethodInfo interceptorMethod,
                                                                                      NetAspectDefinition aspect, Instruction instruction)
        {
            return Create(method, interceptorMethod, aspect, instruction, (factory, interceptorInfo) => factory.FillAfterSpecific(interceptorInfo));
        }
    }
}