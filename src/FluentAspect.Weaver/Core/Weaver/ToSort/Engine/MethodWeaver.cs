using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Data.Variables.Instructions;
using NetAspect.Weaver.Core.Weaver.Data.Variables.Method;
using NetAspect.Weaver.Core.Weaver.Engine.Instructions;
using NetAspect.Weaver.Core.Weaver.Engine.Lifecycle;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public class MethodWeaver
   {
       private AspectBuilder aspectBuilder;

       public MethodWeaver(AspectBuilder aspectBuilder)
       {
           this.aspectBuilder = aspectBuilder;
       }

       public void Weave(MethodDefinition method,
         WeavingMethodSession weavingMethodSession,
         ErrorHandler errorHandler)
      {
          NetAspectWeavingMethod w;
          VariableDefinition result = CreateMethodResultVariable(method);
           List<VariableDefinition> allVariables = new List<VariableDefinition>(); ;
          if (BuildNetAspectWeavingModel(method, weavingMethodSession, errorHandler, out w, result, allVariables)) return;

          method.Body.Variables.AddRange(allVariables.Where(v => v != null));


         method.Weave(w, result);
      }

       private bool BuildNetAspectWeavingModel(MethodDefinition method, WeavingMethodSession weavingMethodSession,
                                                      ErrorHandler errorHandler, out NetAspectWeavingMethod w,
                                                      VariableDefinition result,
                                                      //out IlInjectorAvailableVariables availableVariables,
                                                      List<VariableDefinition> allVariables)
       {
           w = new NetAspectWeavingMethod();

           

           var instructionsToInsertP_L = new InstructionsToInsert();
           
           List<Instruction> aspectInit = new List<Instruction>();
           if (FillForInstructions(method, weavingMethodSession, errorHandler, w, result, instructionsToInsertP_L, allVariables, aspectInit))
               return true;

           var befores = new List<Instruction>();
           var beforeConstructorBaseCall = new List<Instruction>();
           var afters = new List<Instruction>();
           var onExceptions = new List<Instruction>();
           var onFinallys = new List<Instruction>();
           var variablesForMethod = CreateVariablesForMethod(instructionsToInsertP_L, method, allVariables, result);

           foreach (var aspectInstanceForMethodWeaving in weavingMethodSession.Method)
           {
               variablesForMethod.Aspects.Add(new Variable(instructionsToInsertP_L, new VariableAspect(aspectBuilder, aspectInstanceForMethodWeaving.Aspect.LifeCycle, aspectInstanceForMethodWeaving.Aspect.Type, aspectInstanceForMethodWeaving.Instance), method, null, allVariables));
               aspectInstanceForMethodWeaving.Check(errorHandler, variablesForMethod);
               if (errorHandler.Errors.Any())
                   return true;

               aspectInstanceForMethodWeaving.Inject(befores, afters, onExceptions, onFinallys, variablesForMethod,
                                                beforeConstructorBaseCall);

               var interceptorFactoryInstructions = new List<Instruction>();

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
           return false;
       }

       private VariablesForMethod CreateVariablesForMethod(InstructionsToInsert instructionsToInsert, MethodDefinition method, List<VariableDefinition> variables, VariableDefinition result)
       {
           return new VariablesForMethod(
               new Variable(instructionsToInsert, new VariableCurrentMethodBuilder(), method, null, variables),
               new Variable(instructionsToInsert, new VariableCurrentProperty(), method, null, variables),
               new Variable(instructionsToInsert, new VariableParameters(), method, null, variables),
               new Variable(instructionsToInsert, new VariableException(), method, null, variables),
               new Variable(instructionsToInsert, new ExistingVariable(result), method, null, variables));
       }

       private static void GenerateOnExceptionStatements(VariablesForMethod availableVariables,
                                                         List<Instruction> onExceptionInstructions, IEnumerable<Instruction> onExceptions)
       {
           onExceptionInstructions.Add(Instruction.Create(OpCodes.Stloc, availableVariables.Exception.Definition));
           onExceptionInstructions.AddRange(onExceptions);
           onExceptionInstructions.Add(Instruction.Create(OpCodes.Rethrow));
       }

       private static VariableDefinition CreateMethodResultVariable(MethodDefinition method)
       {
           return method.ReturnType ==
                  method.Module.TypeSystem.Void
                      ? null
                      : new VariableDefinition(method.ReturnType);
       }

       private bool FillForInstructions(MethodDefinition method, WeavingMethodSession weavingMethodSession,
                                               ErrorHandler errorHandler, NetAspectWeavingMethod w, VariableDefinition result,
                                               InstructionsToInsert instructionsToInsertP_L, List<VariableDefinition> allVariables, List<Instruction> aspectInstructions)
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
                       return true;
                   variablesForInstruction.Aspects.Add(new Variable(instructions, new VariableAspect(aspectBuilder, v.Aspect.LifeCycle, v.Aspect.Type, v.Instance), method, instruction.Key, allVariables));
                   v.Weave(aroundInstructionIl, variablesForInstruction);
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
           return false;
       }

       private VariablesForInstruction CreateVariablesForInstruction(InstructionsToInsert instructionsToInsert, MethodDefinition method, List<VariableDefinition> variables, Instruction instruction, VariableDefinition result, WeavingMethodSession model)
       {
           VariablesForMethod variablesForMethod = CreateVariablesForMethod(instructionsToInsert, method, variables, result);
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

    internal class ExistingVariable : Variable.IVariableBuilder
    {
        private readonly VariableDefinition _variable;

        public ExistingVariable(VariableDefinition variable)
        {
            _variable = variable;
        }

        public void Check(MethodDefinition method, ErrorHandler errorHandler)
        {
            
        }

        public VariableDefinition Build(InstructionsToInsert instructionsToInsert_P, MethodDefinition method, Instruction instruction)
        {
            return _variable;
        }
    }
}
