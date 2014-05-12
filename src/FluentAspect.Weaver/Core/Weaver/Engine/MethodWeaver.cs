using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
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

         var result = method.ReturnType ==
                                     method.Module.TypeSystem.Void
                                        ? null
                                        : new VariableDefinition(method.ReturnType);

         var availableVariables = new IlInjectorAvailableVariables(result, method, null);
         var allVariables = new List<VariableDefinition>();

         foreach (var instruction in methodWeavingModel.Instructions)
         {
            var instructionIl = new NetAspectWeavingMethod.InstructionIl();
            w.Instructions.Add(instruction.Key, instructionIl);
            var variablesForInstruction = new IlInjectorAvailableVariables(result, method, instruction.Key);
            var ils = new List<AroundInstructionIl>();
            foreach (var v in instruction.Value)
            {
               v.Check(errorHandler, variablesForInstruction);
               var aroundInstructionIl = new AroundInstructionIl();
               v.Weave(aroundInstructionIl, variablesForInstruction);
               ils.Add(aroundInstructionIl);
            }
            instructionIl.Before.AddRange(variablesForInstruction.calledInstructions);
            instructionIl.Before.AddRange(variablesForInstruction.calledParametersInstructions);
            instructionIl.Before.AddRange(variablesForInstruction.calledParametersObjectInstructions);
            availableVariables.BeforeInstructions.AddRange(variablesForInstruction.BeforeInstructions);
            allVariables.AddRange(variablesForInstruction.Variables);
            foreach (var aroundInstructionIl in ils)
            {
               instructionIl.Before.AddRange(aroundInstructionIl.BeforeInstruction);
               instructionIl.After.AddRange(aroundInstructionIl.AfterInstruction);
            }
            instructionIl.Before.AddRange(variablesForInstruction.recallcalledInstructions);
            instructionIl.Before.AddRange(variablesForInstruction.recallcalledParametersInstructions);
         }

         methodWeavingModel.Method.Befores.Check(errorHandler);
         methodWeavingModel.Method.Afters.Check(errorHandler);
         methodWeavingModel.Method.OnExceptions.Check(errorHandler);
         methodWeavingModel.Method.OnFinallys.Check(errorHandler);
         if (errorHandler.errors.Count > 0)
            return;

         var befores = new List<Instruction>();
         var afters = new List<Instruction>();
         var onExceptions = new List<Instruction>();
         var onFinallys = new List<Instruction>();
         methodWeavingModel.Method.Befores.Inject(befores, availableVariables);
         methodWeavingModel.Method.Afters.Inject(afters, availableVariables);
         methodWeavingModel.Method.OnExceptions.Inject(onExceptions, availableVariables);
         methodWeavingModel.Method.OnFinallys.Inject(onFinallys, availableVariables);


         w.BeforeInstructions.AddRange(availableVariables.BeforeInstructions);
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
