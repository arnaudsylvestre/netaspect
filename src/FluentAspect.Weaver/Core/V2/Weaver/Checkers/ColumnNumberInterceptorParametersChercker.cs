﻿using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Checkers
{
    public class ColumnNumberInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly Instruction instruction;

        public ColumnNumberInterceptorParametersChercker(Instruction instruction)
        {
            this.instruction = instruction;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.SequencePoint(instruction, errorListener, parameter);
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType<int>(parameter, errorListener);
        }
    }
}