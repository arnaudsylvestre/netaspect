using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class Injector : IIlInjector
   {
      private readonly MethodDefinition _method;
       private readonly MethodInfo interceptorMethod;
      private readonly InterceptorParameterConfigurations interceptorParameterConfigurations;


       public Injector(MethodDefinition method_P, MethodInfo interceptorMethod_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
      {
         _method = method_P;
         interceptorMethod = interceptorMethod_P;
           interceptorParameterConfigurations = interceptorParameterConfigurations_P;
      }

      public void Check(ErrorHandler errorHandler)
      {
         interceptorParameterConfigurations.Check(interceptorMethod.GetParameters(), errorHandler);
      }

      public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
      {
          instructions.Add(Instruction.Create(OpCodes.Ldloc, availableInformations.InterceptorVariable));
         ParametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations, interceptorParameterConfigurations);
         instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
      }
   }
}
