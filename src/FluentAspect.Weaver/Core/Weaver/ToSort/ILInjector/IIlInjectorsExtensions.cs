using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
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

      public static void Inject<T>(this IEnumerable<IIlInjector<T>> injectors, List<Mono.Cecil.Cil.Instruction> instructions, T info)
      {
         foreach (var ilInjector in injectors)
         {
            ilInjector.Inject(instructions, info);
         }
      }
   }
}
