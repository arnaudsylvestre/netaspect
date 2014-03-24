using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.V2.Weaver.Call;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Generators
{
    public class SequencePointIntInterceptorParametersIlGenerator<T> : IInterceptorParameterIlGenerator<T>
    {
        private Instruction instruction;
        private Func<SequencePoint, int> sequencePointInfoProvider;

        public SequencePointIntInterceptorParametersIlGenerator(Instruction instruction, Func<SequencePoint, int> sequencePointInfoProvider)
        {
            this.instruction = instruction;
            this.sequencePointInfoProvider = sequencePointInfoProvider;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
        {
            instructions.Add(InstructionFactory.Create(instruction.GetLastSequencePoint(),
                                                                      sequencePointInfoProvider));
        }
    }
    public class SequencePointStringInterceptorParametersIlGenerator<T> : IInterceptorParameterIlGenerator<T>
    {
        private Instruction instruction;
        private Func<SequencePoint, string> sequencePointInfoProvider;

        public SequencePointStringInterceptorParametersIlGenerator(Instruction instruction, Func<SequencePoint, string> sequencePointInfoProvider)
        {
            this.instruction = instruction;
            this.sequencePointInfoProvider = sequencePointInfoProvider;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
        {
            instructions.Add(InstructionFactory.Create(instruction.GetLastSequencePoint(),
                                                                      sequencePointInfoProvider));
        }
    }

    public class FieldInterceptorParametersIlGenerator<T> : IInterceptorParameterIlGenerator<T>
        where T : IlInstructionInjectorAvailableVariables
    {
        private Instruction instruction;
        private ModuleDefinition module;

        public FieldInterceptorParametersIlGenerator(Instruction instruction, ModuleDefinition module)
        {
            this.instruction = instruction;
            this.module = module;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
        {
            var fieldReference = instruction.Operand as FieldReference;
            instructions.AppendCallToThisGetType(module);
            instructions.AppendCallToGetField(fieldReference.Name, module);
            //instructions.Add(Instruction.Create(OpCodes.Ldnull));
        }
    }


    public class InstanceInterceptorParametersIlGenerator<T> : IInterceptorParameterIlGenerator<T>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
        }
    }
    public class CalledInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInstructionInjectorAvailableVariables>
    {
        private Instruction instruction;

        public CalledInterceptorParametersIlGenerator(Instruction instruction)
        {
            this.instruction = instruction;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInstructionInjectorAvailableVariables info)
        {
            if (!info.VariablesCalled.ContainsKey(instruction))
            {
                instructions.Add(Instruction.Create(OpCodes.Ldnull));
            }
            else
                instructions.Add(Instruction.Create(OpCodes.Ldloc, info.VariablesCalled[instruction]));
        }
    }
}