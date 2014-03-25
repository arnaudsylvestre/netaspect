using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weaver.Helpers
{
    public static class IIlInjectorsExtensions
    {
        public static void Check<T>(this IEnumerable<IIlInjector<T>> injectors, ErrorHandler errorHandler, T info)
        {
            foreach (var ilInjector in injectors)
            {
                ilInjector.Check(errorHandler, info);
            }
        }

        public static void Inject<T>(this IEnumerable<IIlInjector<T>> injectors, List<Instruction> instructions, T info)
        {
            foreach (var ilInjector in injectors)
            {
                ilInjector.Inject(instructions, info);
            }
        }
    }
}