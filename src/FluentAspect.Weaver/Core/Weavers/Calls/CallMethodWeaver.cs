using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Model;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.Calls
{
   public class CallMethodWeaver : IWeaveable
   {
      private readonly MethodDefinition _method;
      private readonly Instruction _instruction;
      private readonly List<NetAspectAttribute> _interceptorTypes;

      public CallMethodWeaver(MethodDefinition method, Instruction instruction, List<NetAspectAttribute> interceptorTypes)
      {
         _method = method;
         _instruction = instruction;
         _interceptorTypes = interceptorTypes;
      }

      public void Check(ErrorHandler errorHandler)
      {
         
      }

      public void Weave(ErrorHandler errorP_P)
      {
         var reference = _instruction.Operand as MethodReference;

         var instructions = _method.Body.Instructions;
         SequencePoint point_L = null;
         for (int i = 0; i < instructions.Count; i++)
         {
            if (instructions[i].SequencePoint != null)
               point_L = instructions[i].SequencePoint;
            if (instructions[i] == _instruction)
            {

               var afterInstructions = CreateAfterInstructions(_method.Module, point_L);
               foreach (var beforeInstruction in afterInstructions.Reverse())
               {
                  instructions.Insert(i + 1, beforeInstruction);
               }

               var beforeInstructions = CreateBeforeInstructions(_method.Module);
               
               foreach (var beforeInstruction in beforeInstructions.Reverse())
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

      private IEnumerable<Instruction> CreateAfterInstructions(ModuleDefinition module, SequencePoint instructionP_P)
      {

         var instructions = new List<Instruction>();
         foreach (var interceptorType in _interceptorTypes)
         {
            var afterCallMethod = interceptorType.CallWeavingConfiguration.AfterInterceptor.Method;
            if (afterCallMethod != null)
            {
               var parameters = new Dictionary<string, Action>();
               parameters.Add("linenumber", () => instructions.Add(Instruction.Create(OpCodes.Ldc_I4, instructionP_P == null ? 0 : instructionP_P.StartLine)));
               parameters.Add("columnnumber", () => instructions.Add(Instruction.Create(OpCodes.Ldc_I4, instructionP_P == null ? 0 : instructionP_P.StartColumn)));
               parameters.Add("filename", () => instructions.Add(Instruction.Create(OpCodes.Ldstr, instructionP_P == null ? "" : Path.GetFileName(instructionP_P.Document.Url))));
               parameters.Add("filepath", () => instructions.Add(Instruction.Create(OpCodes.Ldstr, instructionP_P == null ? "" : instructionP_P.Document.Url)));
               parameters.Add("caller", () => instructions.Add(Instruction.Create(OpCodes.Ldarg_0)));

               foreach (var parameter_L in _method.Parameters)
               {
                  ParameterDefinition parameter1_L = parameter_L;
                  parameters.Add((parameter1_L.Name + "Caller").ToLower(), () => instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter1_L)));
               }
                  

               foreach (var parameterInfo_L in afterCallMethod.GetParameters())
               {
                  parameters[parameterInfo_L.Name.ToLower()]();
               }

               instructions.Add(Instruction.Create(OpCodes.Call, module.Import(afterCallMethod)));
            }
         }
         return instructions;
      }

      private IEnumerable<Instruction> CreateBeforeInstructions(ModuleDefinition module)
      {
         var instructions = new List<Instruction>();
         foreach (var interceptorType in _interceptorTypes)
         {
            if (interceptorType.CallWeavingConfiguration.BeforeInterceptor.Method != null)
            {
               instructions.Add(Instruction.Create(OpCodes.Call, module.Import(interceptorType.CallWeavingConfiguration.BeforeInterceptor.Method)));
            }
         }
         return instructions;
      }
   }
}