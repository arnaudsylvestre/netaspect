﻿using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public static class IIlInjectorsExtensions
   {
      public static void Check<T>(this IEnumerable<IIlInjector<T>> injectors, ErrorHandler errorHandler)
      {
         foreach (var ilInjector in injectors)
         {
            ilInjector.Check(errorHandler);
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
