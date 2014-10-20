﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ToSort.Data;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Instructions;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables.Method;
using NetAspect.Weaver.Core.Weaver.ToSort.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.ToSort.Engine.LifeCycle;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine
{
    public static class VariablesFactory
    {

        public static VariablesForMethod CreateVariablesForMethod(InstructionsToInsert instructionsToInsert, MethodDefinition method, List<VariableDefinition> variables, VariableDefinition result)
        {
            return new VariablesForMethod(
                new Variable(instructionsToInsert, new VariableCurrentMethodBuilder(), method, null, variables),
                new Variable(instructionsToInsert, new VariableCurrentProperty(), method, null, variables),
                new Variable(instructionsToInsert, new VariableParameters(), method, null, variables),
                new Variable(instructionsToInsert, new VariableException(), method, null, variables),
                new Variable(instructionsToInsert, new ExistingVariable(result), method, null, variables));
        }
    }

   public class MethodWeaver
   {
       private AspectInstanceBuilder _aspectInstanceBuilder;

       public MethodWeaver(AspectInstanceBuilder _aspectInstanceBuilder)
       {
           this._aspectInstanceBuilder = _aspectInstanceBuilder;
       }

       public void Weave(MethodDefinition methodToWeave,
         WeavingMethodSession weavingMethodSession,
         ErrorHandler errorHandler)
      {
           try
           {
               var result = CreateMethodResultVariable(methodToWeave);
               var allVariables = new List<VariableDefinition>(); ;
               var w = BuildNetAspectWeavingMethod(methodToWeave, weavingMethodSession, errorHandler, result, allVariables);

               methodToWeave.Body.Variables.AddRange(allVariables.Where(v => v != null));


               methodToWeave.Weave(w, result);

           }
           catch
           {

           }
      }

       private NetAspectWeavingMethod BuildNetAspectWeavingMethod(MethodDefinition method, WeavingMethodSession weavingMethodSession,
                                                      ErrorHandler errorHandler, 
                                                      VariableDefinition result,
                                                      //out IlInjectorAvailableVariables availableVariables,
                                                      List<VariableDefinition> allVariables)
       {

           var w = new NetAspectWeavingMethod();

           var instructionsToInsertP_L = new InstructionsToInsert();
           
           var aspectInit = new List<Mono.Cecil.Cil.Instruction>();
           FillForInstructions(method, weavingMethodSession, errorHandler, w, result, instructionsToInsertP_L,
                               allVariables, aspectInit);

           var befores = new List<Mono.Cecil.Cil.Instruction>();
           var beforeConstructorBaseCall = new List<Mono.Cecil.Cil.Instruction>();
           var afters = new List<Mono.Cecil.Cil.Instruction>();
           var onExceptions = new List<Mono.Cecil.Cil.Instruction>();
           var onFinallys = new List<Mono.Cecil.Cil.Instruction>();
           var variablesForMethod = VariablesFactory.CreateVariablesForMethod(instructionsToInsertP_L, method, allVariables, result);

           foreach (var aspectInstanceForMethodWeaving in weavingMethodSession.Method)
           {
               variablesForMethod.Aspects.Add(new Variable(instructionsToInsertP_L, new VariableAspect(_aspectInstanceBuilder, aspectInstanceForMethodWeaving.Aspect.LifeCycle, aspectInstanceForMethodWeaving.Aspect.Type, aspectInstanceForMethodWeaving.Instance), method, null, allVariables));
               aspectInstanceForMethodWeaving.Check(errorHandler, variablesForMethod);
               if (errorHandler.Errors.Any())
                   throw new Exception();

               aspectInstanceForMethodWeaving.Inject(befores, afters, onExceptions, onFinallys, variablesForMethod,
                                                beforeConstructorBaseCall);

               var interceptorFactoryInstructions = new List<Mono.Cecil.Cil.Instruction>();

               if (beforeConstructorBaseCall.Any())
               {
                   beforeConstructorBaseCall.InsertRange(0, interceptorFactoryInstructions);
               }
               else if (befores.Any() || afters.Any() || onExceptions.Any() || onFinallys.Any())
               {
                   befores.InsertRange(0, interceptorFactoryInstructions);
               }
               
           }


           
           w.BeforeConstructorBaseCall.AddRange(beforeConstructorBaseCall);
           w.BeforeInstructions.AddRange(instructionsToInsertP_L.aspectInitialisation);
           w.BeforeInstructions.AddRange(instructionsToInsertP_L.BeforeInstructions);
           w.BeforeInstructions.AddRange(befores);
           w.BeforeInstructions.AddRange(aspectInit);
           w.AfterInstructions.AddRange(afters);
           if (onExceptions.Any())
           {
               GenerateOnExceptionStatements(variablesForMethod, w.OnExceptionInstructions, onExceptions);
           }
           w.OnFinallyInstructions.AddRange(onFinallys);
           return w;
       }


       private static void GenerateOnExceptionStatements(VariablesForMethod availableVariables,
                                                         List<Mono.Cecil.Cil.Instruction> onExceptionInstructions, IEnumerable<Mono.Cecil.Cil.Instruction> onExceptions)
       {
           onExceptionInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Stloc, availableVariables.Exception.Definition));
           onExceptionInstructions.AddRange(onExceptions);
           onExceptionInstructions.Add(Mono.Cecil.Cil.Instruction.Create(OpCodes.Rethrow));
       }

       private static VariableDefinition CreateMethodResultVariable(MethodDefinition method)
       {
           return method.ReturnType ==
                  method.Module.TypeSystem.Void
                      ? null
                      : new VariableDefinition(method.ReturnType);
       }

       private void FillForInstructions(MethodDefinition method, WeavingMethodSession weavingMethodSession,
                                               ErrorHandler errorHandler, NetAspectWeavingMethod w, VariableDefinition result,
                                               InstructionsToInsert instructionsToInsertP_L, List<VariableDefinition> allVariables, List<Mono.Cecil.Cil.Instruction> aspectInstructions)
       {
           foreach (var instruction in weavingMethodSession.Instructions)
           {
               var instructionIl = new NetAspectWeavingMethod.InstructionIl();
               w.Instructions.Add(instruction.Key, instructionIl);
               var instructions = new InstructionsToInsert();
               instructions.aspectInitialisation = aspectInstructions;
               //var variablesForInstruction = new IlInjectorAvailableVariables(result, method, instruction.Key, instructions);
               var variablesForInstruction = CreateVariablesForInstruction(instructions, method, allVariables, instruction.Key, result, weavingMethodSession);
               
               var ils = new List<AroundInstructionIl>();
               foreach (var v in instruction.Value)
               {
                   var aroundInstructionIl = new AroundInstructionIl(); 
                   

                   v.Check(errorHandler, variablesForInstruction);
                   if (errorHandler.Errors.Any())
                       throw new Exception();
                   variablesForInstruction.Aspects.Add(new Variable(instructions, new VariableAspect(_aspectInstanceBuilder, v.Aspect.LifeCycle, v.Aspect.Type, v.Instance), method, instruction.Key, allVariables));
                   v.Inject(aroundInstructionIl, variablesForInstruction);
                   ils.Add(aroundInstructionIl);
               }
               instructionIl.Before.AddRange(instructions.calledParametersInstructions);
               instructionIl.Before.AddRange(instructions.calledInstructions);
               instructionIl.Before.AddRange(instructions.calledParametersObjectInstructions);
               instructionIl.After.AddRange(instructions.resultInstructions);
               instructionsToInsertP_L.BeforeInstructions.AddRange(instructions.BeforeInstructions);
               foreach (AroundInstructionIl aroundInstructionIl in ils)
               {
                   instructionIl.Before.AddRange(aroundInstructionIl.BeforeInstruction);
                   instructionIl.After.AddRange(aroundInstructionIl.AfterInstruction);
               }
               instructionIl.Before.AddRange(instructions.recallcalledInstructions);
               instructionIl.Before.AddRange(instructions.recallcalledParametersInstructions);
           }
       }

       private VariablesForInstruction CreateVariablesForInstruction(InstructionsToInsert instructionsToInsert, MethodDefinition method, List<VariableDefinition> variables, Mono.Cecil.Cil.Instruction instruction, VariableDefinition result, WeavingMethodSession model)
       {
           VariablesForMethod variablesForMethod = VariablesFactory.CreateVariablesForMethod(instructionsToInsert, method, variables, result);
           var calledParameters = new MultipleVariable(instructionsToInsert, new VariablesCalledParameters(), method, instruction, variables);
           return new VariablesForInstruction(instruction,
               variablesForMethod.CallerMethod,
               variablesForMethod.CallerProperty,
               variablesForMethod.Parameters,
               variablesForMethod.Exception,
               calledParameters,
               new Variable(instructionsToInsert, new VariableCalled(() => calledParameters.Definitions ), method, instruction, variables ),
               new Variable(instructionsToInsert, new VariableFieldValue(), method, instruction, variables), 
               variablesForMethod.Result,
               new Variable(instructionsToInsert, new VariableResultForInstruction(), method, instruction, variables),
               new Variable(instructionsToInsert, new VariableCalledParametersObject(() => calledParameters.Definitions), method, instruction, variables));
       }
   }
}
