using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    public static class IIlInjectorsExtensions
    {
        public static void Check<T>(this IEnumerable<IIlInjector> injectors, ErrorHandler errorHandler, T info)
        {
            foreach (var ilInjector in injectors)
            {
                ilInjector.Check(errorHandler);
            }
        }

        public static void Inject(this IEnumerable<IIlInjector> injectors, List<Instruction> instructions, IlInstructionInjectorAvailableVariables info)
        {
            foreach (var ilInjector in injectors)
            {
                ilInjector.Inject(instructions, info);
            }
        }
    }
}