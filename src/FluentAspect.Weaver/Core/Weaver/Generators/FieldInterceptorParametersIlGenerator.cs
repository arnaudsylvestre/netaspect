using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public class FieldInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInjectorAvailableVariablesForInstruction>
    {
        private Instruction instruction;
        private ModuleDefinition module;

        public FieldInterceptorParametersIlGenerator(Instruction instruction, ModuleDefinition module)
        {
            this.instruction = instruction;
            this.module = module;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariablesForInstruction info)
        {
            var fieldReference = instruction.Operand as FieldReference;
            instructions.AppendCallToTargetGetType(module, info.Called);
            instructions.AppendCallToGetField(fieldReference.Name, module);
            //instructions.Add(Instruction.Create(OpCodes.Ldnull));
        }
    }

    public class PropertyPInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInjectorAvailableVariablesForInstruction>
    {
        private PropertyDefinition property;
        private ModuleDefinition module;

        public PropertyPInterceptorParametersIlGenerator(PropertyDefinition property, ModuleDefinition module)
        {
            this.property = property;
            this.module = module;
        }

        public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariablesForInstruction info)
        {
            instructions.AppendCallToTargetGetType(module, info.Called);
            instructions.AppendCallToGetProperty(property.Name, module);
            //instructions.Add(Instruction.Create(OpCodes.Ldnull));
        }
    }
}