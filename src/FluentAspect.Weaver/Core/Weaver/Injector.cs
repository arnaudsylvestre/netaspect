using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.ILInjector;

namespace NetAspect.Weaver.Core.Weaver
{
    public class Injector<T> : IIlInjector<T> where T : VariablesForMethod
    {
      private readonly MethodDefinition _method;
      private readonly MethodInfo interceptorMethod;
      private readonly InterceptorParameterConfigurations<T> interceptorParameterConfigurations;
      private readonly IWevingPreconditionInjector<T> weavingPreconditionInjector;


      public Injector(MethodDefinition method_P, MethodInfo interceptorMethod_P, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P, IWevingPreconditionInjector<T> weavingPreconditionInjector)
      {
         _method = method_P;
         interceptorMethod = interceptorMethod_P;
         interceptorParameterConfigurations = interceptorParameterConfigurations_P;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public void Check(ErrorHandler errorHandler)
      {
         if (interceptorMethod.ReturnType != typeof (void))
            errorHandler.OnError(ErrorCode.InterceptorMustBeVoid, FileLocation.None, interceptorMethod.Name, interceptorMethod.DeclaringType.FullName);
         interceptorParameterConfigurations.Check(interceptorMethod.GetParameters(), errorHandler);
      }

      public void Inject(List<Instruction> instructions, T availableInformations)
      {
         Instruction end = Instruction.Create(OpCodes.Nop);
         var precondition = new List<Instruction>();
         weavingPreconditionInjector.Inject(precondition, availableInformations, interceptorMethod, _method);
         if (precondition.Any())
         {
            instructions.AddRange(precondition);
            instructions.Add(Instruction.Create(OpCodes.Brfalse, end));
         }
         // TODO : InterceptorVariable doit etre multiple 
         instructions.Add(Instruction.Create(OpCodes.Ldloc, availableInformations.Aspect.Definition));
         ParametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations, interceptorParameterConfigurations);
         instructions.Add(Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
         instructions.Add(end);
      }
   }
}
