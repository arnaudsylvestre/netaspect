﻿using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using NetAspect.Core;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model;
using NetAspect.Weaver.Core.Weaver.Call;
using NetAspect.Weaver.Core.Weaver.Helpers;
using NetAspect.Weaver.Core.Weaver.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver
{
    public class AroundInstructionIl
    {
        public readonly List<Instruction> BeforeInstruction = new List<Instruction>();
        public readonly List<Instruction> AfterInstruction = new List<Instruction>();
    }

    public class AroundMethodIl
    {
        public List<VariableDefinition> Variables;
        public List<Instruction> InitBeforeInstruction;
        public List<Instruction> BeforeMethod;
        public List<Instruction> AfterMethod;
        public List<Instruction> OnException;
        public List<Instruction> OnFinally;
    }

    public interface IAroundInstructionWeaver
    {
        void Weave(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables, Instruction instruction);
        void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction variables);
    }

    public class AroundInstructionWeaver : IAroundInstructionWeaver
    {
        private IIlInjectorInitializer<IlInjectorAvailableVariablesForInstruction> initializer;
        private IIlInjector<IlInjectorAvailableVariablesForInstruction> before;
        private IIlInjector<IlInjectorAvailableVariablesForInstruction> after;

        public AroundInstructionWeaver(IIlInjectorInitializer<IlInjectorAvailableVariablesForInstruction> initializer, IIlInjector<IlInjectorAvailableVariablesForInstruction> before, IIlInjector<IlInjectorAvailableVariablesForInstruction> after)
        {
            this.initializer = initializer;
            this.before = before;
            this.after = after;
        }

        public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction variables)
        {
            before.Check(errorHandler, variables);
            after.Check(errorHandler, variables);
        }

        public void Weave(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables, Instruction instruction)
        {
            initializer.Inject(il, variables, instruction);
            before.Inject(il.BeforeInstruction, variables);
            after.Inject(il.AfterInstruction, variables);
        }
    }

    public class CallGetFieldInitializerWeaver : IIlInjectorInitializer<IlInjectorAvailableVariablesForInstruction>
    {
        public void Inject(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables, Instruction instruction)
        {
            //if (instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda)
            //{
            //    var fieldReference = (instruction.Operand as FieldReference).Resolve();
            //    var called = new VariableDefinition(fieldReference.DeclaringType);
            //    il.Variables.Add(called);
            //    variables.VariablesCalled.Add(instruction, called);
            //    il.InitBeforeInstruction.Add(Instruction.Create(OpCodes.Stloc, called));
            //    il.JustBeforeInstruction.Add(Instruction.Create(OpCodes.Ldloc, called));
            //}
        }
    }

    

    public class AroundMethodWeaver
    {
        public void Weave2(NetAspect.Weaver.Helpers.IL.Method method, WeavingModel weavingModel,
                          ErrorHandler errorHandler)
        {
            var w = new NetAspectWeavingMethod();

            VariableDefinition result = method.MethodDefinition.ReturnType ==
                                        method.MethodDefinition.Module.TypeSystem.Void
                                            ? null
                                            : new VariableDefinition(method.MethodDefinition.ReturnType);

            var availableVariables = new IlInjectorAvailableVariables(result, method.MethodDefinition);
           var allVariables = new List<VariableDefinition>();

            foreach (var instruction in weavingModel.Instructions)
            {
                var instructionIl = new NetAspectWeavingMethod.InstructionIl();
                w.Instructions.Add(instruction.Key, instructionIl);
                var variablesForInstruction = new IlInjectorAvailableVariablesForInstruction(w, availableVariables, instruction.Key);
                var ils = new List<AroundInstructionIl>();
                foreach (var v in instruction.Value)
                {
                    v.Check(errorHandler, variablesForInstruction);
                    var aroundInstructionIl = new AroundInstructionIl();
                    v.Weave(aroundInstructionIl, variablesForInstruction, instruction.Key);
                    ils.Add(aroundInstructionIl);
                }
                instructionIl.Before.AddRange(variablesForInstruction.calledInstructions);
                instructionIl.Before.AddRange(variablesForInstruction.calledParametersInstructions);
                allVariables.AddRange(variablesForInstruction.Variables);
                foreach (var aroundInstructionIl in ils)
                {
                    instructionIl.Before.AddRange(aroundInstructionIl.BeforeInstruction);
                    instructionIl.After.AddRange(aroundInstructionIl.AfterInstruction);
                }
                instructionIl.Before.AddRange(variablesForInstruction.recallcalledInstructions);
                instructionIl.Before.AddRange(variablesForInstruction.recallcalledParametersInstructions);
            }

            weavingModel.Method.Befores.Check(errorHandler, availableVariables);
            weavingModel.Method.Afters.Check(errorHandler, availableVariables);
            weavingModel.Method.OnExceptions.Check(errorHandler, availableVariables);
            weavingModel.Method.OnFinallys.Check(errorHandler, availableVariables);
            if (errorHandler.Errors.Count > 0)
                return;

            List<Instruction> befores = new List<Instruction>();
            List<Instruction> afters = new List<Instruction>();
            List<Instruction> onExceptions = new List<Instruction>();
            List<Instruction> onFinallys = new List<Instruction>();
           weavingModel.Method.Befores.Inject(befores, availableVariables);
           weavingModel.Method.Afters.Inject(afters, availableVariables);
           weavingModel.Method.OnExceptions.Inject(onExceptions, availableVariables);
           weavingModel.Method.OnFinallys.Inject(onFinallys, availableVariables);


           w.BeforeInstructions.AddRange(availableVariables.BeforeInstructions);
           w.BeforeInstructions.AddRange(befores);
           w.AfterInstructions.AddRange(afters);
           if (onExceptions.Any())
           {
              w.OnExceptionInstructions.Add(Instruction.Create(OpCodes.Stloc, availableVariables.Exception));
              w.OnExceptionInstructions.AddRange(onExceptions);
           }
           w.OnFinallyInstructions.AddRange(onFinallys);

           method.MethodDefinition.Body.Variables.AddRange(allVariables);
           method.MethodDefinition.Body.Variables.AddRange(availableVariables.Variables);

           method.MethodDefinition.Weave(w);
        }


        //public void Weave(NetAspect.Weaver.Helpers.IL.Method method, WeavingModel weavingModel, ErrorHandler errorHandler)
        //{
        //    VariableDefinition result = method.MethodDefinition.ReturnType ==
        //                                method.MethodDefinition.Module.TypeSystem.Void
        //                                    ? null
        //                                    : new VariableDefinition(method.MethodDefinition.ReturnType);


        //    var newInstructions = new Collection<Instruction>();
        //    var variablesForInstructionCall = new IlInjectorAvailableVariables(result, method.MethodDefinition, null);
        //    List<VariableDefinition> variablesToAdd = new List<VariableDefinition>();

        //    AroundInstructionIl il = new AroundInstructionIl();

        //    foreach (Instruction instruction in method.MethodDefinition.Body.Instructions)
        //    {
        //        foreach (var aroundInstructionWeaver in weavingModel.GetAroundInstructionWeavers(instruction))
        //        {
        //            aroundInstructionWeaver.Check(errorHandler, variablesForInstructionCall);
        //            aroundInstructionWeaver.Weave(il, variablesForInstructionCall, instruction);
        //        }
        //        if (weavingModel.BeforeInstructions.ContainsKey(instruction) ||
        //            weavingModel.AfterInstructions.ContainsKey(instruction))
        //        {
        //            var initInstructions = new List<Instruction>();
        //            var recallInstructions = new List<Instruction>();
        //            if (instruction.IsACallInstruction())
        //            {
        //                var calledMethod = instruction.GetCalledMethod();
        //                //variablesForInstructionCall.VariablesByInstruction.Add(instruction, new Dictionary<string, VariableDefinition>());
        //                foreach (var parameter in calledMethod.Parameters.Reverse())
        //                {
        //                    var variableDefinition = new VariableDefinition(parameter.ParameterType);
        //                    //variablesForInstructionCall.VariablesByInstruction[instruction].Add("called" + parameter.Name, variableDefinition);
        //                    initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
        //                }
        //                if (!calledMethod.IsStatic)
        //                {
        //                    var variableDefinition = new VariableDefinition(calledMethod.DeclaringType);
        //                    //variablesForInstructionCall.VariablesCalled.Add(instruction, variableDefinition);
        //                    initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
        //                    recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
        //                }
        //                //foreach (var parameter in calledMethod.Parameters)
        //                //{
        //                //    recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variablesForInstructionCall.VariablesByInstruction[instruction]["called" + parameter.Name]));
        //                //}
        //            }
        //            if (instruction.IsAnAccessField())
        //            {
        //                if (instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda)
        //                {
        //                    var fieldReference = (instruction.Operand as FieldReference).Resolve();
        //                    var variableDefinition = new VariableDefinition(fieldReference.DeclaringType);
        //                    variablesToAdd.Add(variableDefinition);
        //                    //variablesForInstructionCall.VariablesCalled.Add(instruction, variableDefinition);
        //                    initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
        //                    recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
        //                }
        //            }
        //            if (instruction.IsAnUpdateField())
        //            {
        //                if (instruction.OpCode == OpCodes.Stfld)
        //                {
        //                    var fieldReference = (instruction.Operand as FieldReference).Resolve();
        //                    var variableDefinition = new VariableDefinition(fieldReference.DeclaringType);
        //                    var valueDefinition = new VariableDefinition(fieldReference.FieldType);
        //                    variablesToAdd.Add(variableDefinition);
        //                    variablesToAdd.Add(valueDefinition);
        //                    //variablesForInstructionCall.VariablesCalled.Add(instruction, variableDefinition);
        //                    initInstructions.Add(Instruction.Create(OpCodes.Stloc, valueDefinition));
        //                    initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
        //                    recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
        //                    initInstructions.Add(Instruction.Create(OpCodes.Ldloc, valueDefinition));
        //                }
        //            }
        //            InstructionExtensions.AddRange(newInstructions, initInstructions);
        //            InstructionExtensions.AddRange(newInstructions, recallInstructions);
        //        }

        //        if (weavingModel.BeforeInstructions.ContainsKey(instruction))
        //        {
        //            var instructionIlInjector = weavingModel.BeforeInstructions[instruction];
        //            instructionIlInjector.Check(errorHandler, variablesForInstructionCall);
        //            var beforeInstruction = new List<Instruction>();
        //            instructionIlInjector.Inject(beforeInstruction, variablesForInstructionCall);
        //            //InstructionExtensions.AddRange(newInstructions, variablesForInstructionCall.Instructions);
        //            InstructionExtensions.AddRange(newInstructions, beforeInstruction);
        //        }
        //        newInstructions.Add(instruction);

        //        if (weavingModel.AfterInstructions.ContainsKey(instruction))
        //        {
        //            var instructionIlInjector = weavingModel.AfterInstructions[instruction];
        //            instructionIlInjector.Check(errorHandler, variablesForInstructionCall);
        //            var beforeInstruction = new List<Instruction>();
        //            instructionIlInjector.Inject(beforeInstruction, variablesForInstructionCall);
        //            //InstructionExtensions.AddRange(newInstructions, variablesForInstructionCall.Instructions);
        //            InstructionExtensions.AddRange(newInstructions, beforeInstruction);
        //        }
        //    }
        //    var variables = new IlInjectorAvailableVariables(result, method.MethodDefinition, null);
        //    MethodWeavingModel methodWeavingModel = weavingModel.Method;

        //    if (result != null)
        //        method.MethodDefinition.Body.Variables.Add(result);
            
        //    foreach (var variableDefinition in variablesToAdd)
        //    {
        //        method.MethodDefinition.Body.Variables.Add(variableDefinition);
        //    }

        //    method.MethodDefinition.Body.Instructions.Clear();
        //    InstructionExtensions.AddRange(method.MethodDefinition.Body.Instructions, newInstructions);
        //    if (!methodWeavingModel.Befores.Any() && !methodWeavingModel.Afters.Any() &&
        //        !methodWeavingModel.OnExceptions.Any() && !methodWeavingModel.OnFinallys.Any())
        //    {
        //        Finalize(method.MethodDefinition);
        //        return;
        //    }
        //    var beforeInstructions = new List<Instruction>();
        //    Instruction beforeAfter = Instruction.Create(OpCodes.Nop);
        //    var afterInstructions = new List<Instruction>();
        //    var onExceptionInstructions = new List<Instruction>();
        //    var onFinallyInstructions = new List<Instruction>();
        //    Collection<Instruction> bodyInstructions = method.MethodDefinition.Body.Instructions;
        //    var beforeAllInstructions = new Collection<Instruction>();
        //    var methodInstructions = new Collection<Instruction>();
        //    bool callBaseConstructorDone = !method.MethodDefinition.IsConstructor;
        //    foreach (Instruction bodyInstruction in bodyInstructions)
        //    {
        //        if (callBaseConstructorDone)
        //            methodInstructions.Add(bodyInstruction);
        //        else
        //        {
        //            beforeAllInstructions.Add(bodyInstruction);
        //            //if (IsCallInstruction(bodyInstruction))
        //            //{
        //            //    if (bodyInstruction.Operand is MethodReference)
        //            //    {
        //            //        var methodReference = bodyInstruction.Operand as MethodReference;
        //            //        if (methodReference.Name == ".ctor")
        //            //            callBaseConstructorDone = true;
        //            //    }
        //            //}
        //        }
        //    }
        //    if (methodInstructions.Count == 0)
        //        methodInstructions.Add(Instruction.Create(OpCodes.Nop));
        //    var BeforeExceptionManagementInstructions = new List<Instruction>();
        //    var AfterExceptionManagementInstructions = new List<Instruction>();

        //    List<Instruction> end = InstructionsHelper.FixReturns(method.MethodDefinition, variables.Result,
        //                                                          null, beforeAfter);


        //    methodWeavingModel.Befores.Inject(beforeInstructions, variables);
        //    methodWeavingModel.OnExceptions.Inject(onExceptionInstructions, variables);
        //    methodWeavingModel.Afters.Inject(afterInstructions, variables);
        //    methodWeavingModel.OnFinallys.Inject(onFinallyInstructions, variables);
        //    var allInstructions = new List<Instruction>();
        //    allInstructions.AddRange(beforeAllInstructions);
        //    //allInstructions.AddRange(variables.Instructions);
        //    allInstructions.AddRange(beforeInstructions);
        //    allInstructions.AddRange(methodInstructions);
        //    if (end.Count != 0)
        //    {
        //        allInstructions.Add(beforeAfter);
        //        allInstructions.AddRange(afterInstructions);
        //        Instruction gotoEnd = Instruction.Create(OpCodes.Leave, end.First());
        //        allInstructions.Add(gotoEnd);
        //    }


        //    if (onExceptionInstructions.Any())
        //    {
        //        BeforeExceptionManagementInstructions.Add(Instruction.Create(OpCodes.Stloc, variables.Exception));
        //        AfterExceptionManagementInstructions.Add(Instruction.Create(OpCodes.Rethrow));
        //    }
        //    if (end.Count > 0)
        //        onFinallyInstructions.Add(Instruction.Create(OpCodes.Endfinally));

        //    allInstructions.AddRange(BeforeExceptionManagementInstructions);
        //    allInstructions.AddRange(onExceptionInstructions);
        //    allInstructions.AddRange(AfterExceptionManagementInstructions);
        //    allInstructions.AddRange(onFinallyInstructions);
        //    allInstructions.AddRange(end);

        //    method.MethodDefinition.Body.Instructions.Clear();
        //    InstructionExtensions.AddRange(method.MethodDefinition.Body.Instructions, allInstructions);


        //    if (onExceptionInstructions.Any())
        //    {
        //        Instruction lastCatchException = null;
        //        if (onFinallyInstructions.Any())
        //            lastCatchException = onFinallyInstructions.First();
        //        else
        //        {
        //            if (end.Count != 0)
        //                lastCatchException = end.First();
        //        }

        //        method.AddTryCatch(methodInstructions.First(), BeforeExceptionManagementInstructions.First(),
        //                           BeforeExceptionManagementInstructions.First(), lastCatchException);
        //    }

        //    if (onFinallyInstructions.Any())
        //        method.AddTryFinally(methodInstructions.First(), onFinallyInstructions.First(),
        //                             onFinallyInstructions.First(), end.Count > 0 ? end.First() : null);


        //    Finalize(method.MethodDefinition);
        //}

        private static void Finalize(MethodDefinition methodDefinition)
        {
            methodDefinition.Body.InitLocals = methodDefinition.Body.HasVariables;
        }
    }
}