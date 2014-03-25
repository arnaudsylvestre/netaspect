﻿using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weaver.Checkers
{
    public class FilenameInterceptorParametersChercker : IInterceptorParameterChecker
    {
        private readonly Instruction instruction;

        public FilenameInterceptorParametersChercker(Instruction instruction)
        {
            this.instruction = instruction;
        }

        public void Check(ParameterInfo parameter, ErrorHandler errorListener)
        {
            Ensure.SequencePoint(instruction, errorListener, parameter);
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType<string>(parameter, errorListener);
        }
    }
}