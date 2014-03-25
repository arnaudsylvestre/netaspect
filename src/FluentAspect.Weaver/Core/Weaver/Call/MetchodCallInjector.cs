using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.V2.Weaver.Engine;
using FluentAspect.Weaver.Core.V2.Weaver.Generators;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Call
{
    public class MetchodCallInjector : IIlInjector<IlInstructionInjectorAvailableVariables>
    {
        private readonly Type _aspectType;
        private readonly MethodDefinition _method;
        private readonly ParametersIlGenerator<IlInstructionInjectorAvailableVariables> ilGenerator;
        private readonly MethodInfo interceptorMethod;
        private readonly ParametersChecker _parametersChecker;

        public MetchodCallInjector(MethodDefinition method, Type aspectType, ParametersChecker parametersChecker,
                                   ParametersIlGenerator<IlInstructionInjectorAvailableVariables> ilGenerator,
                                   MethodInfo interceptorMethod)
        {
            _method = method;
            _aspectType = aspectType;
            _parametersChecker = parametersChecker;
            this.ilGenerator = ilGenerator;
            this.interceptorMethod = interceptorMethod;
        }

        public void Check(ErrorHandler errorHandler, IlInstructionInjectorAvailableVariables availableInformations)
        {
            _parametersChecker.Check(interceptorMethod.GetParameters(), errorHandler);
        }

        public void Inject(List<Instruction> instructions, IlInstructionInjectorAvailableVariables availableInformations)
        {
            VariableDefinition interceptor = _method.CreateVariable(_aspectType);
            instructions.AppendCreateNewObject(interceptor, _aspectType, _method.Module);
            instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptor));
            ilGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations);
            instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
        }
    }
}