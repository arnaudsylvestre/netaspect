using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Methods;
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

        public MethodWeavingBeforeMethodInjector(MethodDefinition method_P, MethodInfo interceptorMethod_P, Type aspectType)
       {
          _method = method_P;
          interceptorMethod = interceptorMethod_P;
           _aspectType = aspectType;
       }

       public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariables availableInformations)
       {
           var checker = new ParametersChecker();
           checker.CreateCheckerForInstanceParameter(_method);
          checker.Check(interceptorMethod.GetParameters(), errorHandler);
       }

       public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
       {
           var parametersIlGenerator = new ParametersIlGenerator();
           parametersIlGenerator.CreateIlGeneratorForInstanceParameter(_method);
           var interceptor = _method.CreateVariable(_aspectType);
           instructions.AppendCreateNewObject(interceptor, _aspectType, _method.Module);
           instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptor));
               parametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations);
               instructions.Add(
                  Instruction.Create(
                     OpCodes.Call,
                     _method.Module.Import(
                        interceptorMethod)));

        }
    }
}