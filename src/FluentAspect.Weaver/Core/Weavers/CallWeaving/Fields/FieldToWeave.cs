﻿using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{

    public class FieldToWeave
    {
        public JoinPoint JoinPoint { get; set; }
        public IEnumerable<CallWeavingConfiguration> Interceptors { get; set; }
        public FieldReference Field
        {
            get { return JoinPoint.Instruction.Operand as FieldReference; }
        }
    }
}