using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Call;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public class FieldInterceptorParametersIlGenerator<T> : IInterceptorParameterIlGenerator<T>
        where T : IlInstructionInjectorAvailableVariables
    {
        private Instruction instruction;
        private ModuleDefinition module;

        public FieldInterceptorParametersIlGenerator(Instruction instruction, ModuleDefinition module)
        {
            this.instruction = instruction;
            this.module = module;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, T info)
        {
            var fieldReference = instruction.Operand as FieldReference;
            instructions.AppendCallToThisGetType(module);
            instructions.AppendCallToGetField(fieldReference.Name, module);
            //instructions.Add(Instruction.Create(OpCodes.Ldnull));
        }
    }
}