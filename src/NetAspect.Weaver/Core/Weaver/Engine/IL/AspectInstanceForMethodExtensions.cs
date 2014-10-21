using System.Collections.Generic;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;
using NetAspect.Weaver.Core.Weaver.ToSort.ILInjector;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Engine
{
   public static class AspectInstanceForMethodExtensions
   {
       public static void Check(this AspectInstanceForMethod aspectInstanceForMethod, ErrorHandler errorHandler, VariablesForMethod availableVariables, Variable aspectInstance)
      {
          aspectInstanceForMethod.Befores.Check(errorHandler, availableVariables, aspectInstance);
          aspectInstanceForMethod.BeforeConstructorBaseCalls.Check(errorHandler, availableVariables, aspectInstance);
          aspectInstanceForMethod.Afters.Check(errorHandler, availableVariables, aspectInstance);
          aspectInstanceForMethod.OnExceptions.Check(errorHandler, availableVariables, aspectInstance);
          aspectInstanceForMethod.OnFinallys.Check(errorHandler, availableVariables, aspectInstance);
      }

      public static void Inject(this AspectInstanceForMethod aspectInstanceForInstruction, List<Mono.Cecil.Cil.Instruction> befores, List<Mono.Cecil.Cil.Instruction> afters, List<Mono.Cecil.Cil.Instruction> onExceptions, List<Mono.Cecil.Cil.Instruction> onFinallys, VariablesForMethod availableVariables, List<Mono.Cecil.Cil.Instruction> beforeConstructorBaseCall_P, Variable aspectInstance)
      {
          aspectInstanceForInstruction.BeforeConstructorBaseCalls.Inject(beforeConstructorBaseCall_P, availableVariables, aspectInstance);
          aspectInstanceForInstruction.Befores.Inject(befores, availableVariables, aspectInstance);
          aspectInstanceForInstruction.Afters.Inject(afters, availableVariables, aspectInstance);
          aspectInstanceForInstruction.OnExceptions.Inject(onExceptions, availableVariables, aspectInstance);
          aspectInstanceForInstruction.OnFinallys.Inject(onFinallys, availableVariables, aspectInstance);
      }
   }
}
