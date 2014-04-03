using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Helpers;
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
            instructions.Add(InstructionFactory.Create(instruction.GetLastSequencePoint(),
                                                       sequencePointInfoProvider));
        }
    }
}