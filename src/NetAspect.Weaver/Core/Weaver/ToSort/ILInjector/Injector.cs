using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Checkers;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
    public class Injector<T> : IIlInjector<T> where T : VariablesForMethod
    {
        private readonly MethodDefinition _method;
        private readonly IEnumerable<MethodInfo> interceptorMethods;
        private MethodInfo interceptorMethod;
      private readonly InterceptorParameterPossibilities<T> _interceptorParameterPossibilities;
      private readonly IWeavingPreconditionInjector<T> weavingPreconditionInjector;


      public Injector(MethodDefinition method_P, IEnumerable<MethodInfo> interceptorMethod_P, InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP, IWeavingPreconditionInjector<T> weavingPreconditionInjector)
      {
         _method = method_P;
         interceptorMethods = interceptorMethod_P;
         _interceptorParameterPossibilities = interceptorParameterPossibilitiesP;
         this.weavingPreconditionInjector = weavingPreconditionInjector;
      }

      public void Check(ErrorHandler errorHandlers, T availableInformations, Variable aspectInstance)
      {
          foreach (var interceptorMethod in interceptorMethods)
          {
              var interceptorHandler = new ErrorHandler();
              Check(aspectInstance, interceptorMethod, interceptorHandler);
              if (!interceptorHandler.Errors.Any())
              {
                  this.interceptorMethod = interceptorMethod;
                  return;
                  
              }
          }

          Check(aspectInstance, interceptorMethods.First(), errorHandlers);
      }

        private void Check(Variable aspectInstance, MethodInfo interceptorMethod, ErrorHandler interceptorHandler)
        {
            if (interceptorMethod.ReturnType != typeof (void))
                interceptorHandler.OnError(ErrorCode.InterceptorMustBeVoid, FileLocation.None, interceptorMethod.Name,
                                           interceptorMethod.DeclaringType.FullName);
            _interceptorParameterPossibilities.Check(interceptorMethod.GetParameters(), interceptorHandler, interceptorMethod);
            aspectInstance.Check(interceptorHandler);
        }

        public void Inject(List<Mono.Cecil.Cil.Instruction> instructions, T availableInformations, Variable aspectInstance)
        {
            Mono.Cecil.Cil.Instruction end = Mono.Cecil.Cil.Instruction.Create(OpCodes.Nop);
            var precondition = new List<Mono.Cecil.Cil.Instruction>();
            weavingPreconditionInjector.Inject(precondition, availableInformations, _method);
            if (precondition.Any())
            {
                instructions.AddRange(precondition);
                instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Brfalse, end));
            }
            instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Ldloc, aspectInstance.Definition));
            ParametersIlGenerator.Generate(interceptorMethod.GetParameters(), instructions, availableInformations,
                                           _interceptorParameterPossibilities);
            instructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Call, _method.Module.Import(interceptorMethod)));
            instructions.Add(end);
        }
    }
}
