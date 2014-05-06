using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class Injector : IIlInjector
   {
      private readonly MethodDefinition _method;
      private readonly NetAspectDefinition aspect;
      private readonly MethodInfo interceptorMethod;
      private readonly InterceptorParameterConfigurations interceptorParameterConfigurations;
      private AspectBuilder aspectBuilder;


      public Injector(MethodDefinition method_P,
         MethodInfo interceptorMethod_P,
         NetAspectDefinition aspect,
         InterceptorParameterConfigurations interceptorParameterConfigurations_P, AspectBuilder aspectBuilder_P)
      {
         _method = method_P;
         interceptorMethod = interceptorMethod_P;
         this.aspect = aspect;
         interceptorParameterConfigurations = interceptorParameterConfigurations_P;
         aspectBuilder = aspectBuilder_P;
      }

      public void Check(ErrorHandler errorHandler)
      {
         interceptorParameterConfigurations.Check(interceptorMethod.GetParameters(), errorHandler);
      }

      public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
      {
         ParametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations, interceptorParameterConfigurations);

         var interceptor = new VariableDefinition(_method.Module.Import(aspect.Type));
         availableInformations.Variables.Add(interceptor);
         aspectBuilder.CreateInterceptor(aspect, _method, interceptor, instructions);
         instructions.Add(Instruction.Create(OpCodes.Ldloc, interceptor));
         instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
      }
   }
}
