using System.Collections.Generic;
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
    }
}