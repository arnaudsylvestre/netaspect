using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Generators
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