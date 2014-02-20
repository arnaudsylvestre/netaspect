using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.V2
{
   public class AroundMethodWeaver
   {
      IlInjectorAvailableVariables variables;

      public void Weave(Method method, MethodWeavingModel methodWeavingModel, ErrorHandler errorHandlerP_P)
      {

         VariableDefinition result = method.MethodDefinition.ReturnType == method.MethodDefinition.Module.TypeSystem.Void ? null : new VariableDefinition(method.MethodDefinition.ReturnType);
         variables = new IlInjectorAvailableVariables(result, method.MethodDefinition);
          SimpleErrorListener errorListener = new SimpleErrorListener();
         methodWeavingModel.Befores.Check(errorHandlerP_P, variables);
         methodWeavingModel.Afters.Check(errorHandlerP_P, variables);
         methodWeavingModel.OnExceptions.Check(errorHandlerP_P, variables);
         methodWeavingModel.OnFinallys.Check(errorHandlerP_P, variables);
          if (errorListener.HasError)
              return;
         if (!methodWeavingModel.Befores.Any() && !methodWeavingModel.Afters.Any() &&
            !methodWeavingModel.OnExceptions.Any() && !methodWeavingModel.OnFinallys.Any())
         {
            return;
         }
         method.MethodDefinition.Body.InitLocals = true;
         var beforeInstructions = new List<Instruction>();
         var beforeAfter = Instruction.Create(OpCodes.Nop);
         var afterInstructions = new List<Instruction>();
         var onExceptionInstructions = new List<Instruction>();
         var onFinallyInstructions = new List<Instruction>();
         var methodInstructions = new Collection<Instruction>(method.MethodDefinition.Body.Instructions);

         var end = InstructionsHelper.FixReturns(method.MethodDefinition, this.variables.Result, methodInstructions, beforeAfter);

         methodWeavingModel.Befores.Inject(beforeInstructions, variables);
         methodWeavingModel.OnExceptions.Inject(onExceptionInstructions, variables);
         methodWeavingModel.Afters.Inject(afterInstructions, variables);
         methodWeavingModel.OnFinallys.Inject(onFinallyInstructions, variables);
         var allInstructions = new List<Instruction>();
         allInstructions.AddRange(variables.Instructions);
         allInstructions.AddRange(beforeInstructions);
         allInstructions.AddRange(methodInstructions);
         if (end.Count != 0)
         {
            allInstructions.Add(beforeAfter);
            allInstructions.AddRange(afterInstructions);
            var gotoEnd = Instruction.Create(OpCodes.Leave, end.First());
            allInstructions.Add(gotoEnd);
         }


         allInstructions.AddRange(onExceptionInstructions);
         allInstructions.AddRange(onFinallyInstructions);
         allInstructions.AddRange(end);

         method.MethodDefinition.Body.Instructions.Clear();
         method.MethodDefinition.Body.Instructions.AddRange(allInstructions);


         if (onExceptionInstructions.Any())
         {
            Instruction lastCatchException = null;
            if (onFinallyInstructions.Any())
               lastCatchException = onFinallyInstructions.First();
            else
            {
               if (end.Count != 0)
                  lastCatchException = end.First();
            }

            method.AddTryCatch(methodInstructions.First(), onExceptionInstructions.First(), onExceptionInstructions.First(), lastCatchException);
         }

         if (onFinallyInstructions.Any())
            method.AddTryFinally(methodInstructions.First(), onFinallyInstructions.First(), onFinallyInstructions.First(), end.Count > 0 ? end.First() : null);
      }
   }
}