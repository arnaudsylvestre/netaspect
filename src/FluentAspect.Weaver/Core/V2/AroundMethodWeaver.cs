﻿using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.V2
{
   public class AroundMethodWeaver
   {
      public void Weave(Method method, WeavingModel weavingModel, ErrorHandler errorHandler)
      {

         var result = method.MethodDefinition.ReturnType == method.MethodDefinition.Module.TypeSystem.Void ? null : new VariableDefinition(method.MethodDefinition.ReturnType);
         var variables = new IlInjectorAvailableVariables(result, method.MethodDefinition);

          var newInstructions = new Collection<Instruction>();
         foreach (var instruction in method.MethodDefinition.Body.Instructions)
          {
              if (weavingModel.BeforeInstructions.ContainsKey(instruction))
              {
                  var instructionIlInjector = weavingModel.BeforeInstructions[instruction];
                  instructionIlInjector.Check(errorHandler, variables);
                  var beforeInstruction = new List<Instruction>();
                  instructionIlInjector.Inject(beforeInstruction, variables);
                  newInstructions.AddRange(beforeInstruction);
              }
             newInstructions.Add(instruction);

             if (weavingModel.AfterInstructions.ContainsKey(instruction))
             {
                 var instructionIlInjector = weavingModel.AfterInstructions[instruction];
                 instructionIlInjector.Check(errorHandler, variables);
                 var beforeInstruction = new List<Instruction>();
                 instructionIlInjector.Inject(beforeInstruction, variables);
                 newInstructions.AddRange(beforeInstruction);
             }
          }
         method.MethodDefinition.Body.Instructions.Clear();
          method.MethodDefinition.Body.Instructions.AddRange(newInstructions);

          var methodWeavingModel = weavingModel.Method;

          if (result != null)
            method.MethodDefinition.Body.Variables.Add(result);
         methodWeavingModel.Befores.Check(errorHandler, variables);
         methodWeavingModel.Afters.Check(errorHandler, variables);
         methodWeavingModel.OnExceptions.Check(errorHandler, variables);
         methodWeavingModel.OnFinallys.Check(errorHandler, variables);
         if (errorHandler.Errors.Count > 0)
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
         var bodyInstructions = method.MethodDefinition.Body.Instructions;
         var beforeAllInstructions = new Collection<Instruction>();
         var methodInstructions = new Collection<Instruction>();
          var callBaseConstructorDone = !method.MethodDefinition.IsConstructor;
          foreach (var bodyInstruction in bodyInstructions)
          {
              if (callBaseConstructorDone)
                methodInstructions.Add(bodyInstruction);
              else
              {
                  beforeAllInstructions.Add(bodyInstruction);
                  if (IsCallInstruction(bodyInstruction))
                  {
                      if (bodyInstruction.Operand is MethodReference)
                      {
                          var methodReference = bodyInstruction.Operand as MethodReference;
                          if (methodReference.Name == ".ctor")
                              callBaseConstructorDone = true;
                      }
                  }

              }
          }
          if (methodInstructions.Count == 0)
              methodInstructions.Add(Instruction.Create(OpCodes.Nop));
          var BeforeExceptionManagementInstructions = new List<Instruction>();
          var AfterExceptionManagementInstructions = new List<Instruction>();

         var end = InstructionsHelper.FixReturns(method.MethodDefinition, variables.Result, methodInstructions, beforeAfter);


         methodWeavingModel.Befores.Inject(beforeInstructions, variables);
         methodWeavingModel.OnExceptions.Inject(onExceptionInstructions, variables);
         methodWeavingModel.Afters.Inject(afterInstructions, variables);
         methodWeavingModel.OnFinallys.Inject(onFinallyInstructions, variables);
         var allInstructions = new List<Instruction>();
          allInstructions.AddRange(beforeAllInstructions);
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


         if (onExceptionInstructions.Any())
         {
             BeforeExceptionManagementInstructions.Add(Instruction.Create(OpCodes.Stloc, variables.Exception));
             AfterExceptionManagementInstructions.Add(Instruction.Create(OpCodes.Rethrow));
         }
          if (end.Count > 0)
              onFinallyInstructions.Add(Instruction.Create(OpCodes.Endfinally));

         allInstructions.AddRange(BeforeExceptionManagementInstructions);
         allInstructions.AddRange(onExceptionInstructions);
         allInstructions.AddRange(AfterExceptionManagementInstructions);
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

            method.AddTryCatch(methodInstructions.First(), BeforeExceptionManagementInstructions.First(), BeforeExceptionManagementInstructions.First(), lastCatchException);
         }

         if (onFinallyInstructions.Any())
            method.AddTryFinally(methodInstructions.First(), onFinallyInstructions.First(), onFinallyInstructions.First(), end.Count > 0 ? end.First() : null);
      }

       private static bool IsCallInstruction(Instruction bodyInstruction)
       {
           return bodyInstruction.OpCode == OpCodes.Call || bodyInstruction.OpCode == OpCodes.Calli || bodyInstruction.OpCode == OpCodes.Callvirt;
       }
   }
}