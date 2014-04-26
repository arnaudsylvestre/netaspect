using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Generators
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
            SequencePoint instructionPP = instruction.GetLastSequencePoint();
            instructions.Add(Instruction.Create(OpCodes.Ldc_I4,
                                                instructionPP == null
                                                    ? 0
                                                    : sequencePointInfoProvider(instructionPP)));
        }
    }
}