using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ATrier;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public static class MethodWeaver
   {
      public static void Weave(this MethodDefinition method,
         MethodWeavingModel methodWeavingModel,
         ErrorHandler errorHandler)
      {
         var w = new NetAspectWeavingMethod();

         VariableDefinition result = method.ReturnType ==
                                     method.Module.TypeSystem.Void
            ? null
            : new VariableDefinition(method.ReturnType);

         var instructionsToInsertP_L = new InstructionsToInsert();
         var availableVariables = new IlInjectorAvailableVariables(result, method, null, instructionsToInsertP_L);
         var allVariables = new List<VariableDefinition>();

         foreach (var instruction in methodWeavingModel.Instructions)
         {
            var instructionIl = new NetAspectWeavingMethod.InstructionIl();
            w.Instructions.Add(instruction.Key, instructionIl);
            var instructions = new InstructionsToInsert();
            var variablesForInstruction = new IlInjectorAvailableVariables(result, method, instruction.Key, instructions);
            var ils = new List<AroundInstructionIl>();
            foreach (AroundInstructionWeaver v in instruction.Value)
            {
               var aroundInstructionIl = new AroundInstructionIl();
               if (variablesForInstruction.InterceptorVariable == null)
               {
                  variablesForInstruction.InterceptorVariable = v.CreateAspect(aroundInstructionIl);
               }

               v.Check(errorHandler, variablesForInstruction);
               if (errorHandler.Errors.Any())
                  return;
               v.Weave(aroundInstructionIl, variablesForInstruction);
               ils.Add(aroundInstructionIl);
            }
            instructionIl.Before.AddRange(instructions.calledParametersInstructions);
            instructionIl.Before.AddRange(instructions.calledInstructions);
            instructionIl.Before.AddRange(instructions.calledParametersObjectInstructions);
            instructionIl.After.AddRange(instructions.resultInstructions);
            instructionsToInsertP_L.BeforeInstructions.AddRange(instructions.BeforeInstructions);
            allVariables.AddRange(variablesForInstruction.Variables);
            allVariables.Add(variablesForInstruction.InterceptorVariable);
            foreach (AroundInstructionIl aroundInstructionIl in ils)
            {
               instructionIl.Before.AddRange(aroundInstructionIl.BeforeInstruction);
               instructionIl.After.AddRange(aroundInstructionIl.AfterInstruction);
            }
            instructionIl.Before.AddRange(instructions.recallcalledInstructions);
            instructionIl.Before.AddRange(instructions.recallcalledParametersInstructions);
         }

         methodWeavingModel.Method.Check(errorHandler);
         if (errorHandler.Errors.Any())
            return;

         var befores = new List<Instruction>();
         var beforeConstructorBaseCall = new List<Instruction>();
         var afters = new List<Instruction>();
         var onExceptions = new List<Instruction>();
         var onFinallys = new List<Instruction>();

         var interceptorFactoryInstructions = new List<Instruction>();
         availableVariables.InterceptorVariable = methodWeavingModel.Method.CreateAspect(interceptorFactoryInstructions);
         allVariables.Add(availableVariables.InterceptorVariable);
         methodWeavingModel.Method.Inject(befores, afters, onExceptions, onFinallys, availableVariables, beforeConstructorBaseCall);

         if (beforeConstructorBaseCall.Any())
         {
            beforeConstructorBaseCall.InsertRange(0, interceptorFactoryInstructions);
         }
         else if (befores.Any() || afters.Any() || onExceptions.Any() || onFinallys.Any())
         {
            befores.InsertRange(0, interceptorFactoryInstructions);
         }
         w.BeforeConstructorBaseCall.AddRange(beforeConstructorBaseCall);
         w.BeforeInstructions.AddRange(instructionsToInsertP_L.BeforeInstructions);
         w.BeforeInstructions.AddRange(befores);
         w.AfterInstructions.AddRange(afters);
         if (onExceptions.Any())
         {
            w.OnExceptionInstructions.Add(Instruction.Create(OpCodes.Stloc, availableVariables.Exception));
            w.OnExceptionInstructions.AddRange(onExceptions);
            w.OnExceptionInstructions.Add(Instruction.Create(OpCodes.Rethrow));
         }
         w.OnFinallyInstructions.AddRange(onFinallys);

         method.Body.Variables.AddRange(allVariables);
         method.Body.Variables.AddRange(availableVariables.Variables);


         method.Weave(w, result);
      }
   }
}
