using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
   public static class IIlInjectorsExtensions
   {
       public static void Check<T>(this IEnumerable<IIlInjector<T>> injectors, ErrorHandler errorHandler, T info, Variable aspectInstance)
      {
         foreach (var ilInjector in injectors)
         {
            ilInjector.Check(errorHandler, info, aspectInstance);
         }
      }

      public static void Inject<T>(this IEnumerable<IIlInjector<T>> injectors, List<Mono.Cecil.Cil.Instruction> instructions, T info, Variable aspectInstance)
      {
         foreach (var ilInjector in injectors)
         {
             ilInjector.Inject(instructions, info, aspectInstance);
         }
      }
   }
}
