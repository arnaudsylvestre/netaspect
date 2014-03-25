﻿using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Generators
{
    public class InstanceInterceptorParametersIlGenerator<T> : IInterceptorParameterIlGenerator<T>
    {
        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
        }
    }
}