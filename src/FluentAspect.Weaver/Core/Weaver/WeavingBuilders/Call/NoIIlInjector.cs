using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call
{
   public class NoIIlInjector : IIlInjector
   {
      public void Check(ErrorHandler errorHandler)
      {
            
      }

      public void Inject(List<Instruction> instructions, IlInstructionInjectorAvailableVariables availableInformations)
      {
            
      }
   }
}