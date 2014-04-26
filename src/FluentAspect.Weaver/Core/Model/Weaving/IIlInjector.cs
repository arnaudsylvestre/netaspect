﻿using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Model.Weaving
{
    public interface IIlInjector<T>
    {
        void Check(ErrorHandler errorHandler);
        void Inject(List<Instruction> instructions, T availableInformations);
    }
}