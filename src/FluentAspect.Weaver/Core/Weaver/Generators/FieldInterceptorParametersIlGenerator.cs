using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public class FieldInterceptorParametersIlGenerator 
    {
        private Instruction instruction;
        private ModuleDefinition module;

        public FieldInterceptorParametersIlGenerator(Instruction instruction, ModuleDefinition module)
        {
            this.instruction = instruction;
            this.module = module;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
        {
            var fieldReference = instruction.Operand as FieldReference;
            instructions.AppendCallToTargetGetType(module, info.Called);
            instructions.AppendCallToGetField(fieldReference.Name, module);
            //instructions.Add(Instruction.Create(OpCodes.Ldnull));
        }
    }
}