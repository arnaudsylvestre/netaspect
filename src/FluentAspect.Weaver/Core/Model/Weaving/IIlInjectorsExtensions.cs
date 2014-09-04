using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public static class IIlInjectorsExtensions
   {
      public static void Check(this IEnumerable<IIlInjector> injectors, ErrorHandler errorHandler)
      {
         foreach (IIlInjector ilInjector in injectors)
         {
            ilInjector.Check(errorHandler);
         }
      }

      public static void Inject(this IEnumerable<IIlInjector> injectors, List<Instruction> instructions, VariablesForMethod info)
      {
         foreach (IIlInjector ilInjector in injectors)
         {
            ilInjector.Inject(instructions, info);
         }
      }
   }
}
