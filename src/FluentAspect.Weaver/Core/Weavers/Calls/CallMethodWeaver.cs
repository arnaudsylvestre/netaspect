using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core
{
   public class CallMethodWeaver : IWeaveable
   {
      private readonly MethodDefinition _method;
      private readonly Instruction _instruction;
      private readonly List<Type> _interceptorTypes;

      public CallMethodWeaver(MethodDefinition method, Instruction instruction, List<Type> interceptorTypes)
      {
         _method = method;
         _instruction = instruction;
         _interceptorTypes = interceptorTypes;
      }

      public void Weave()
      {
         var reference = _instruction.Operand as MethodReference;

         var instructions = _method.Body.Instructions;
         for (int i = 0; i < instructions.Count; i++)
         {
            if (instructions[i] == _instruction)
            {

               var afterInstructions = CreateAfterInstructions(_method.Module);
               afterInstructions.Reverse();
               foreach (var beforeInstruction in afterInstructions)
               {
                  instructions.Insert(i + 1, beforeInstruction);
               }

               var beforeInstructions = CreateBeforeInstructions(_method.Module);
               beforeInstructions.Reverse();
               foreach (var beforeInstruction in beforeInstructions)
               {
                  instructions.Insert(i, beforeInstruction);
               }
               break;
            }
         }

         foreach (var parameter in reference.Parameters)
         {
            var variableDefinition = new VariableDefinition(parameter.ParameterType);
            _method.Body.Variables.Add(variableDefinition);
         }
      }

      private IEnumerable<Instruction> CreateAfterInstructions(ModuleDefinition module)
      {

         var instructions = new List<Instruction>();
         foreach (var interceptorType in _interceptorTypes)
         {
            if (interceptorType.GetMethod("AfterCall") != null)
            {
               instructions.Add(Instruction.Create(OpCodes.Call, module.Import(interceptorType.GetMethod("AfterCall"))));
            }
         }
         return instructions;
      }

      private List<Instruction> CreateBeforeInstructions(ModuleDefinition module)
      {
         var instructions = new List<Instruction>();
         foreach (var interceptorType in _interceptorTypes)
         {
            if (interceptorType.GetMethod("BeforeCall") != null)
            {
               instructions.Add(Instruction.Create(OpCodes.Call, module.Import(interceptorType.GetMethod("BeforeCall"))));
            }
         }
         return instructions;
      }
   }
}