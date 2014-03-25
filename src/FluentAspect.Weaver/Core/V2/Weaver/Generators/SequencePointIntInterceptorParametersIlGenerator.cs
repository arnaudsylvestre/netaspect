using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Helpers;
using FluentAspect.Weaver.Helpers.IL;
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
}