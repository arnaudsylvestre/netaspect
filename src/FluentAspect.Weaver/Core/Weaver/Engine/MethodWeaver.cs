using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
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
          NetAspectWeavingMethod w;
          VariableDefinition result = CreateMethodResultVariable(method);
          IlInjectorAvailableVariables availableVariables;
          List<VariableDefinition> allVariables = new List<VariableDefinition>(); ;
          if (BuildNetAspectWeavingModel(method, methodWeavingModel, errorHandler, out w, result, out availableVariables, allVariables)) return;

          method.Body.Variables.AddRange(allVariables);
         method.Body.Variables.AddRange(availableVariables.Variables);


         method.Weave(w, result);
      }

       private static bool BuildNetAspectWeavingModel(MethodDefinition method, MethodWeavingModel methodWeavingModel,
                                                      ErrorHandler errorHandler, out NetAspectWeavingMethod w,
                                                      VariableDefinition result,
                                                      out IlInjectorAvailableVariables availableVariables,
                                                      List<VariableDefinition> allVariables)
       {
           w = new NetAspectWeavingMethod();

           

           var instructionsToInsertP_L = new InstructionsToInsert();
           availableVariables = new IlInjectorAvailableVariables(result, method, null, instructionsToInsertP_L);
           

           if (FillForInstructions(method, methodWeavingModel, errorHandler, w, result, instructionsToInsertP_L, allVariables))
               return true;

           methodWeavingModel.Method.Check(errorHandler);
           if (errorHandler.Errors.Any())
               return true;

           var befores = new List<Instruction>();
           var beforeConstructorBaseCall = new List<Instruction>();
           var afters = new List<Instruction>();
           var onExceptions = new List<Instruction>();
           var onFinallys = new List<Instruction>();

           var interceptorFactoryInstructions = new List<Instruction>();
           availableVariables.InterceptorVariable = methodWeavingModel.Method.CreateAspect(interceptorFactoryInstructions);
           allVariables.Add(availableVariables.InterceptorVariable);
           methodWeavingModel.Method.Inject(befores, afters, onExceptions, onFinallys, availableVariables,
                                            beforeConstructorBaseCall);

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
           return false;
       }

       private static VariableDefinition CreateMethodResultVariable(MethodDefinition method)
       {
           return method.ReturnType ==
                  method.Module.TypeSystem.Void
                      ? null
                      : new VariableDefinition(method.ReturnType);
       }

       private static bool FillForInstructions(MethodDefinition method, MethodWeavingModel methodWeavingModel,
                                               ErrorHandler errorHandler, NetAspectWeavingMethod w, VariableDefinition result,
                                               InstructionsToInsert instructionsToInsertP_L, List<VariableDefinition> allVariables)
       {
           foreach (var instruction in methodWeavingModel.Instructions)
           {
               var instructionIl = new NetAspectWeavingMethod.InstructionIl();
               w.Instructions.Add(instruction.Key, instructionIl);
               var instructions = new InstructionsToInsert();
               var variablesForInstruction = new IlInjectorAvailableVariables(result, method, instruction.Key, instructions);
               var ils = new List<AroundInstructionIl>();
               foreach (var v in instruction.Value)
               {
                   var aroundInstructionIl = new AroundInstructionIl();
                   if (variablesForInstruction.InterceptorVariable == null)
                   {
                       variablesForInstruction.InterceptorVariable = v.CreateAspect(aroundInstructionIl);
                   }

                   v.Check(errorHandler, variablesForInstruction);
                   if (errorHandler.Errors.Any())
                       return true;
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
           return false;
       }
   }
}
