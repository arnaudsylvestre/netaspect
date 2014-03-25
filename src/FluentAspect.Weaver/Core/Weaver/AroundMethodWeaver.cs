using System.Collections.Generic;
using System.Linq;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weaver.Helpers;
using FluentAspect.Weaver.Core.Weaver.Method;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Core.Weaver
{
    public class AroundInstructionIl
    {
        public List<VariableDefinition> Variables;
        public List<Instruction> InitBeforeInstruction;
        public List<Instruction> BeforeInstruction;
        public List<Instruction> AfterInstruction;
        public List<Instruction> BeforeMethodInstructions;
        public Instruction Instruction;
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
        void Weave(AroundInstructionIl il);
    }

    public class CallGetFieldWeaver : IAroundInstructionWeaver
    {
        public void Weave(AroundInstructionIl il)
        {
            -------------------------------------------
        }
    }

    public class AroundMethodWeaver
    {
        public void Weave2(FluentAspect.Weaver.Helpers.IL.Method method, WeavingModel weavingModel,
                           ErrorHandler errorHandler)
        {
        }


        public void Weave(FluentAspect.Weaver.Helpers.IL.Method method, WeavingModel weavingModel, ErrorHandler errorHandler)
        {
            VariableDefinition result = method.MethodDefinition.ReturnType ==
                                        method.MethodDefinition.Module.TypeSystem.Void
                                            ? null
                                            : new VariableDefinition(method.MethodDefinition.ReturnType);


            var newInstructions = new Collection<Instruction>();
            var variablesForInstructionCall = new IlInjectorAvailableVariables(result, method.MethodDefinition);
            List<VariableDefinition> variablesToAdd = new List<VariableDefinition>();
            foreach (Instruction instruction in method.MethodDefinition.Body.Instructions)
            {
                if (weavingModel.BeforeInstructions.ContainsKey(instruction) ||
                    weavingModel.AfterInstructions.ContainsKey(instruction))
                {
                    var initInstructions = new List<Instruction>();
                    var recallInstructions = new List<Instruction>();
                    if (instruction.IsACallInstruction())
                    {
                        var calledMethod = instruction.GetCalledMethod();
                        variablesForInstructionCall.VariablesByInstruction.Add(instruction, new Dictionary<string, VariableDefinition>());
                        foreach (var parameter in calledMethod.Parameters.Reverse())
                        {
                            var variableDefinition = new VariableDefinition(parameter.ParameterType);
                            variablesForInstructionCall.VariablesByInstruction[instruction].Add("called" + parameter.Name, variableDefinition);
                            initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
                        }
                        if (!calledMethod.IsStatic)
                        {
                            var variableDefinition = new VariableDefinition(calledMethod.DeclaringType);
                            variablesForInstructionCall.VariablesCalled.Add(instruction, variableDefinition);
                            initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
                            recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
                        }
                        foreach (var parameter in calledMethod.Parameters)
                        {
                            recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variablesForInstructionCall.VariablesByInstruction[instruction]["called" + parameter.Name]));
                        }
                    }
                    if (instruction.IsAnAccessField())
                    {
                        if (instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda)
                        {
                            var fieldReference = (instruction.Operand as FieldReference).Resolve();
                            var variableDefinition = new VariableDefinition(fieldReference.DeclaringType);
                            variablesToAdd.Add(variableDefinition);
                            variablesForInstructionCall.VariablesCalled.Add(instruction, variableDefinition);
                            initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
                            recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
                        }
                    }
                    if (instruction.IsAnUpdateField())
                    {
                        if (instruction.OpCode == OpCodes.Stfld)
                        {
                            var fieldReference = (instruction.Operand as FieldReference).Resolve();
                            var variableDefinition = new VariableDefinition(fieldReference.DeclaringType);
                            var valueDefinition = new VariableDefinition(fieldReference.FieldType);
                            variablesToAdd.Add(variableDefinition);
                            variablesToAdd.Add(valueDefinition);
                            variablesForInstructionCall.VariablesCalled.Add(instruction, variableDefinition);
                            initInstructions.Add(Instruction.Create(OpCodes.Stloc, valueDefinition));
                            initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
                            recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
                            initInstructions.Add(Instruction.Create(OpCodes.Ldloc, valueDefinition));
                        }
                    }
                    newInstructions.AddRange(initInstructions);
                    newInstructions.AddRange(recallInstructions);
                }

                if (weavingModel.BeforeInstructions.ContainsKey(instruction))
                {
                    var instructionIlInjector = weavingModel.BeforeInstructions[instruction];
                    instructionIlInjector.Check(errorHandler, variablesForInstructionCall);
                    var beforeInstruction = new List<Instruction>();
                    instructionIlInjector.Inject(beforeInstruction, variablesForInstructionCall);
                    newInstructions.AddRange(variablesForInstructionCall.Instructions);
                    newInstructions.AddRange(beforeInstruction);
                }
                newInstructions.Add(instruction);

                if (weavingModel.AfterInstructions.ContainsKey(instruction))
                {
                    var instructionIlInjector = weavingModel.AfterInstructions[instruction];
                    instructionIlInjector.Check(errorHandler, variablesForInstructionCall);
                    var beforeInstruction = new List<Instruction>();
                    instructionIlInjector.Inject(beforeInstruction, variablesForInstructionCall);
                    newInstructions.AddRange(variablesForInstructionCall.Instructions);
                    newInstructions.AddRange(beforeInstruction);
                }
            }
            var variables = new IlInjectorAvailableVariables(result, method.MethodDefinition);
            MethodWeavingModel methodWeavingModel = weavingModel.Method;

            if (result != null)
                method.MethodDefinition.Body.Variables.Add(result);
            methodWeavingModel.Befores.Check(errorHandler, variables);
            methodWeavingModel.Afters.Check(errorHandler, variables);
            methodWeavingModel.OnExceptions.Check(errorHandler, variables);
            methodWeavingModel.OnFinallys.Check(errorHandler, variables);
            if (errorHandler.Errors.Count > 0)
                return;

            foreach (var variableDefinition in variablesToAdd)
            {
                method.MethodDefinition.Body.Variables.Add(variableDefinition);
            }

            method.MethodDefinition.Body.Instructions.Clear();
            method.MethodDefinition.Body.Instructions.AddRange(newInstructions);
            if (!methodWeavingModel.Befores.Any() && !methodWeavingModel.Afters.Any() &&
                !methodWeavingModel.OnExceptions.Any() && !methodWeavingModel.OnFinallys.Any())
            {
                Finalize(method.MethodDefinition);
                return;
            }
            var beforeInstructions = new List<Instruction>();
            Instruction beforeAfter = Instruction.Create(OpCodes.Nop);
            var afterInstructions = new List<Instruction>();
            var onExceptionInstructions = new List<Instruction>();
            var onFinallyInstructions = new List<Instruction>();
            Collection<Instruction> bodyInstructions = method.MethodDefinition.Body.Instructions;
            var beforeAllInstructions = new Collection<Instruction>();
            var methodInstructions = new Collection<Instruction>();
            bool callBaseConstructorDone = !method.MethodDefinition.IsConstructor;
            foreach (Instruction bodyInstruction in bodyInstructions)
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

            List<Instruction> end = InstructionsHelper.FixReturns(method.MethodDefinition, variables.Result,
                                                                  methodInstructions, beforeAfter);


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
                Instruction gotoEnd = Instruction.Create(OpCodes.Leave, end.First());
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

                method.AddTryCatch(methodInstructions.First(), BeforeExceptionManagementInstructions.First(),
                                   BeforeExceptionManagementInstructions.First(), lastCatchException);
            }

            if (onFinallyInstructions.Any())
                method.AddTryFinally(methodInstructions.First(), onFinallyInstructions.First(),
                                     onFinallyInstructions.First(), end.Count > 0 ? end.First() : null);


            Finalize(method.MethodDefinition);
        }

        private static void Finalize(MethodDefinition methodDefinition)
        {
            methodDefinition.Body.InitLocals = methodDefinition.Body.HasVariables;
        }

        private static bool IsCallInstruction(Instruction bodyInstruction)
        {
            return bodyInstruction.OpCode == OpCodes.Call || bodyInstruction.OpCode == OpCodes.Calli ||
                   bodyInstruction.OpCode == OpCodes.Callvirt;
        }
    }
}