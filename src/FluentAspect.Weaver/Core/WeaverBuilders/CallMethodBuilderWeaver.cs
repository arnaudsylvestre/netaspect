using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Core;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Weavers.Methods;
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

                    var afterInstructions = CreateAfterInstructions();
                    afterInstructions.Reverse();
                    foreach (var beforeInstruction in afterInstructions)
                    {
                        instructions.Insert(i + 1, beforeInstruction);
                    }

                    var beforeInstructions = CreateBeforeInstructions();
                    beforeInstructions.Reverse();
                    foreach (var beforeInstruction in beforeInstructions)
                    {
                        instructions.Insert(i, beforeInstruction);
                    }
                }
            }

            foreach (var parameter in reference.Parameters)
            {
                var variableDefinition = new VariableDefinition(parameter.ParameterType);
                _method.Body.Variables.Add(variableDefinition);
            }
        }

        private IEnumerable<Instruction> CreateAfterInstructions()
        {

            var instructions = new List<Instruction>();
            il.Emit(OpCodes.Newobj, module.Import(interceptorType.GetConstructors()[0]));
            il.Emit(OpCodes.Stloc, interceptor);
            foreach (var interceptorType in _interceptorTypes)
            {
                if (interceptorType.GetMethod("AfterCall") != null)
                {
                    instructions.Add(Instruction.Create(OpCodes.Call));
                }
            }
            return instructions;
        }

        private List<Instruction> CreateBeforeInstructions()
        {
            foreach (var interceptorType in _interceptorTypes)
            {
                if (interceptorType.GetMethod("BeforeCall") != null)
                {

                }

            }
        }
    }

   public class CallMethodBuilderWeaver : IWeaverBuilder
   {
       public IEnumerable<IWeaveable> BuildWeavers(AssemblyDefinition assemblyDefinition, WeavingConfiguration configuration, ErrorHandler errorHandler)
      {
         List<IWeaveable> weavers = new List<IWeaveable>();
         var methods = assemblyDefinition.GetAllMethods();

           foreach (var method in methods)
           {
               if (!method.HasBody)
                   continue;
               foreach (var instruction in method.Body.Instructions)
               {
                   if (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference)
                   {
                       foreach (var methodMatch in configuration.Methods)
                       {
                           if (methodMatch.Matcher(new MethodDefinitionAdapter(instruction.Operand as MethodReference)))
                           {
                               var actualInterceptors = new List<Type>();

                               foreach (var interceptorType in methodMatch.InterceptorTypes)
                               {
                                   if (interceptorType.GetMethod("BeforeCall") != null || interceptorType.GetMethod("AfterCall") != null)
                                       actualInterceptors.Add(interceptorType);
                               }
                               if (actualInterceptors.Count != 0)
                                weavers.Add(new CallMethodWeaver(method, instruction, methodMatch.InterceptorTypes));
                           }
                       }
                   }
               }
           }

         return weavers;
      }
   }
}