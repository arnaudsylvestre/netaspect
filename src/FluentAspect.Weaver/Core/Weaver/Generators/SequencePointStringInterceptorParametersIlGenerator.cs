using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Helpers;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
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
}