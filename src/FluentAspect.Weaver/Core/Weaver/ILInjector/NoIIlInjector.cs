using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Model.Weaving
{
   public class NoIIlInjector<T> : IIlInjector<T>
   {
       public void Check(ErrorHandler errorHandler, T info)
      {
      }

      public void Inject(List<Instruction> instructions, T availableInformations)
      {
      }
   }
}
