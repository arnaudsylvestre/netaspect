using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using FluentAspect.Weaver.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
    public class MethodWeavingMethodInjectorFactory
    {
        public static IIlInjector CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, Type aspectType)
        {

            var checker = new ParametersChecker();
            FillCommon(method, checker);


            var parametersIlGenerator = new ParametersIlGenerator();
            FillCommon(method, parametersIlGenerator);

            return new MethodWeavingBeforeMethodInjector(method, interceptorMethod, aspectType, checker, parametersIlGenerator);
        }

        private static void FillCommon(MethodDefinition method, ParametersIlGenerator parametersIlGenerator)
        {
            parametersIlGenerator.CreateIlGeneratorForInstanceParameter(method);
            parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker)
        {
            checker.CreateCheckerForInstanceParameter(method);
            checker.CreateCheckerForMethodParameter();
            checker.CreateCheckerForParameterNameParameter(method);
            checker.CreateCheckerForParametersParameter();
        }

        public static IIlInjector CreateForAfter(MethodDefinition method, MethodInfo interceptorMethod, Type aspectType)
        {

            var checker = new ParametersChecker();
            FillCommon(method, checker);
            checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator();
            FillCommon(method, parametersIlGenerator);
            parametersIlGenerator.CreateIlGeneratorForResultParameter();
            return new MethodWeavingBeforeMethodInjector(method, interceptorMethod, aspectType, checker, parametersIlGenerator);
        }

        public static IIlInjector CreateForOnException(MethodDefinition method, MethodInfo methodInfo, Type aspectType)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);
            checker.CreateCheckerForExceptionParameter();


            var parametersIlGenerator = new ParametersIlGenerator();
            FillCommon(method, parametersIlGenerator);
            parametersIlGenerator.CreateIlGeneratorForExceptionParameter();
            return new MethodWeavingBeforeMethodInjector(method, methodInfo, aspectType, checker, parametersIlGenerator);
        }
    }

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