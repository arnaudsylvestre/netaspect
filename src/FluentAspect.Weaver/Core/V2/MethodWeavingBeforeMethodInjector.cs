using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class MethodWeavingBeforeMethodInjector : IIlInjector
    {
       private readonly MethodDefinition _method;
       private MethodInfo interceptorMethod;
        private readonly Type _aspectType;
        private ParametersChecker parametersChecker;
        private ParametersIlGenerator ilGenerator;


        public MethodWeavingBeforeMethodInjector(MethodDefinition method_P, MethodInfo interceptorMethod_P, Type aspectType, ParametersChecker parametersChecker, ParametersIlGenerator ilGenerator)
       {
          _method = method_P;
          interceptorMethod = interceptorMethod_P;
           _aspectType = aspectType;
            this.parametersChecker = parametersChecker;
            this.ilGenerator = ilGenerator;
       }

       public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariables availableInformations)
       {
           parametersChecker.Check(interceptorMethod.GetParameters(), errorHandler);
       }

       public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
       {
           var interceptor = _method.CreateVariable(_aspectType);
           instructions.AppendCreateNewObject(interceptor, _aspectType, _method.Module);
           instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptor));
           ilGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations);
           instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));

        }
    }
}