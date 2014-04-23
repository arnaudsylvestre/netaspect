using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class MethodWeavingBeforeMethodInjector : IIlInjector
    {
       private readonly MethodDefinition _method;
       private readonly ParametersIlGenerator<IlInstructionInjectorAvailableVariables> ilGenerator;
        private readonly MethodInfo interceptorMethod;
        private readonly ParametersChecker parametersChecker;
       private AspectBuilder aspectBuilder;
        private NetAspectDefinition aspect;


        public MethodWeavingBeforeMethodInjector(MethodDefinition method_P, MethodInfo interceptorMethod_P, ParametersChecker parametersChecker,
                                                 ParametersIlGenerator<IlInstructionInjectorAvailableVariables> ilGenerator, NetAspectDefinition aspect)
        {
            _method = method_P;
            interceptorMethod = interceptorMethod_P;
           this.parametersChecker = parametersChecker;
            this.ilGenerator = ilGenerator;
            this.aspect = aspect;
        }

        public void Check(ErrorHandler errorHandler)
        {
            parametersChecker.Check(interceptorMethod.GetParameters(), errorHandler);
        }

        public void Inject(List<Instruction> instructions, IlInstructionInjectorAvailableVariables availableInformations)
        {
             
            ilGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations);
            aspectBuilder.CreateInterceptor(aspect, _method, availableInformations);
            instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
        }
    }
}