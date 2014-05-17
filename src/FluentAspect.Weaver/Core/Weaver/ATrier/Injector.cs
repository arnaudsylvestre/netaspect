using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Detectors.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
    public interface IWevingPreconditionInjector
    {
        void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations);
    }

    public class OverrideWevingPreconditionInjector : IWevingPreconditionInjector
    {
        public void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations)
        {
            77777throw new System.NotImplementedException();
        }
    }

    public class NoWevingPreconditionInjector : IWevingPreconditionInjector
    {
        public void Inject(List<Instruction> precondition, IlInjectorAvailableVariables availableInformations)
        {
            
        }
    }

   public class Injector : IIlInjector
   {
      private readonly MethodDefinition _method;
       private readonly MethodInfo interceptorMethod;
      private readonly InterceptorParameterConfigurations interceptorParameterConfigurations;
       private IWevingPreconditionInjector weavingPreconditionInjector;


       public Injector(MethodDefinition method_P, MethodInfo interceptorMethod_P, InterceptorParameterConfigurations interceptorParameterConfigurations_P, IWevingPreconditionInjector weavingPreconditionInjector)
      {
         _method = method_P;
         interceptorMethod = interceptorMethod_P;
           interceptorParameterConfigurations = interceptorParameterConfigurations_P;
           this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public void Check(ErrorHandler errorHandler)
      {
         interceptorParameterConfigurations.Check(interceptorMethod.GetParameters(), errorHandler);
      }

      public void Inject(List<Instruction> instructions, IlInjectorAvailableVariables availableInformations)
      {
          Instruction end = Instruction.Create(OpCodes.Nop);
          var precondition = new List<Instruction>();
          weavingPreconditionInjector.Inject(precondition, availableInformations);
          if (precondition.Any())
          {
              instructions.AddRange(precondition);
              instructions.Add(Instruction.Create(OpCodes.Brfalse, end));
          }
          instructions.Add(Instruction.Create(OpCodes.Ldloc, availableInformations.InterceptorVariable));
         ParametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations, interceptorParameterConfigurations);
         instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
          instructions.Add(end);
      }
   }
}
