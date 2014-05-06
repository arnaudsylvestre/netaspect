using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
   public class PropertyPInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator
   {
      private PropertyDefinition property;
      private ModuleDefinition module;

      public PropertyPInterceptorParametersIlGenerator(PropertyDefinition property, ModuleDefinition module)
      {
         this.property = property;
         this.module = module;
      }

      public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariables info)
      {
         instructions.AppendCallToTargetGetType(module, info.Called);
         instructions.AppendCallToGetProperty(property.Name, module);
         //instructions.Add(Instruction.Create(OpCodes.Ldnull));
      }
   }
}