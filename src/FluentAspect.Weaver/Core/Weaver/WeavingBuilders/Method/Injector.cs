using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.Generators;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class Injector : IIlInjector
    {
       private readonly MethodDefinition _method;
        private readonly MethodInfo interceptorMethod;
       private AspectBuilder aspectBuilder;
        private NetAspectDefinition aspect;
      private readonly InterceptorParameterConfigurations interceptorParameterConfigurations;


      public Injector(MethodDefinition method_P, MethodInfo interceptorMethod_P,
                                                 NetAspectDefinition aspect, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            _method = method_P;
            interceptorMethod = interceptorMethod_P;
            this.aspect = aspect;
         interceptorParameterConfigurations = interceptorParameterConfigurations_P;
        }

        public void Check(ErrorHandler errorHandler)
        {
            ParametersChecker.Check(interceptorMethod.GetParameters(), errorHandler, interceptorParameterConfigurations);
        }

        public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
        {

           ParametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations, interceptorParameterConfigurations);
            aspectBuilder.CreateInterceptor(aspect, _method, availableInformations);
            instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
        }
    }
}