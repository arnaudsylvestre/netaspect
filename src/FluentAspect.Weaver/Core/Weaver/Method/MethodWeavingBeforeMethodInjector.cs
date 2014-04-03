﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Method
{
    public class MethodWeavingBeforeMethodInjector<T> : IIlInjector<T>
    {
        private readonly Type _aspectType;
        private readonly MethodDefinition _method;
        private readonly ParametersIlGenerator<T> ilGenerator;
        private readonly MethodInfo interceptorMethod;
        private readonly ParametersChecker parametersChecker;


        public MethodWeavingBeforeMethodInjector(MethodDefinition method_P, MethodInfo interceptorMethod_P,
                                                 Type aspectType, ParametersChecker parametersChecker,
                                                 ParametersIlGenerator<T> ilGenerator)
        {
            _method = method_P;
            interceptorMethod = interceptorMethod_P;
            _aspectType = aspectType;
            this.parametersChecker = parametersChecker;
            this.ilGenerator = ilGenerator;
        }

        public void Check(ErrorHandler errorHandler, T availableInformations)
        {
            parametersChecker.Check(interceptorMethod.GetParameters(), errorHandler);
        }

        public void Inject(List<Instruction> instructions, T availableInformations)
        {
            VariableDefinition interceptor = _method.CreateVariable(_aspectType);
            instructions.AppendCreateNewObject(interceptor, _aspectType, _method.Module);
            instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptor));
            ilGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations);
            instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
        }
    }
}