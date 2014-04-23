using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Engine
{
   public class CallGetFieldInitializerWeaver : IIlInjectorInitializer<IlInjectorAvailableVariablesForInstruction>
   {
      public void Inject(AroundInstructionIl il, IlInjectorAvailableVariablesForInstruction variables, Instruction instruction)
      {
         //if (instruction.OpCode == OpCodes.Ldfld || instruction.OpCode == OpCodes.Ldflda)
         //{
         //    var fieldReference = (instruction.Operand as FieldReference).Resolve();
         //    var called = new VariableDefinition(fieldReference.DeclaringType);
         //    il.Variables.Add(called);
         //    variables.VariablesCalled.Add(instruction, called);
         //    il.InitBeforeInstruction.Add(Instruction.Create(OpCodes.Stloc, called));
         //    il.JustBeforeInstruction.Add(Instruction.Create(OpCodes.Ldloc, called));
         //}
      }
   }
}