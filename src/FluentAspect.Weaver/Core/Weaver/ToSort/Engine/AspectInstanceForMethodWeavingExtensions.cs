using System.Collections.Generic;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public static class AspectInstanceForMethodWeavingExtensions
   {
       public static void Check(this AspectInstanceForMethodWeaving aspectInstance, ErrorHandler errorHandler, VariablesForMethod availableVariables)
      {
          aspectInstance.Befores.Check(errorHandler, availableVariables);
          aspectInstance.BeforeConstructorBaseCalls.Check(errorHandler, availableVariables);
          aspectInstance.Afters.Check(errorHandler, availableVariables);
          aspectInstance.OnExceptions.Check(errorHandler, availableVariables);
          aspectInstance.OnFinallys.Check(errorHandler, availableVariables);
      }

      public static void Inject(this AspectInstanceForMethodWeaving aspectInstance, List<Instruction> befores, List<Instruction> afters, List<Instruction> onExceptions, List<Instruction> onFinallys, VariablesForMethod availableVariables, List<Instruction> beforeConstructorBaseCall_P)
      {
         aspectInstance.BeforeConstructorBaseCalls.Inject(beforeConstructorBaseCall_P, availableVariables);
         aspectInstance.Befores.Inject(befores, availableVariables);
         aspectInstance.Afters.Inject(afters, availableVariables);
         aspectInstance.OnExceptions.Inject(onExceptions, availableVariables);
         aspectInstance.OnFinallys.Inject(onFinallys, availableVariables);
      }
   }
}
