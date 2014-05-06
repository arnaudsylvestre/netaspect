using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    public static class IIlInjectorsExtensions
    {
        public static void Check(this IEnumerable<IIlInjector> injectors, ErrorHandler errorHandler)
        {
            foreach (var ilInjector in injectors)
            {
                ilInjector.Check(errorHandler);
            }
        }

        public static void Inject(this IEnumerable<IIlInjector> injectors, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            foreach (var ilInjector in injectors)
            {
                ilInjector.Inject(instructions, info);
            }
        }
    }
}