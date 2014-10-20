using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine
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

      public static void Inject(this AspectInstanceForMethodWeaving aspectInstance, List<Mono.Cecil.Cil.Instruction> befores, List<Mono.Cecil.Cil.Instruction> afters, List<Mono.Cecil.Cil.Instruction> onExceptions, List<Mono.Cecil.Cil.Instruction> onFinallys, VariablesForMethod availableVariables, List<Mono.Cecil.Cil.Instruction> beforeConstructorBaseCall_P)
      {
         aspectInstance.BeforeConstructorBaseCalls.Inject(beforeConstructorBaseCall_P, availableVariables);
         aspectInstance.Befores.Inject(befores, availableVariables);
         aspectInstance.Afters.Inject(afters, availableVariables);
         aspectInstance.OnExceptions.Inject(onExceptions, availableVariables);
         aspectInstance.OnFinallys.Inject(onFinallys, availableVariables);
      }
   }
}
