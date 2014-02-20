using System.Collections.Generic;
using FluentAspect.Weaver.Core.Errors;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.V2
{
   public static class IIlInjectorsExtensions
   {
      public static void Check(this IEnumerable<IIlInjector> injectors, ErrorHandler errorHandler, IlInjectorAvailableVariables info)
      {
         foreach (var ilInjector in injectors)
         {
            ilInjector.Check(errorHandler, info);
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