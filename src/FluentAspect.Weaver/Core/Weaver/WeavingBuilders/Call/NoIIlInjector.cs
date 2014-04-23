using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call
{
   public class NoIIlInjector : IIlInjector<IlInjectorAvailableVariablesForInstruction>
   {
      public void Check(ErrorHandler errorHandler, IlInjectorAvailableVariablesForInstruction availableInformations)
      {
            
      }

      public void Inject(List<Instruction> instructions, IlInjectorAvailableVariablesForInstruction availableInformations)
      {
            
      }
   }
}