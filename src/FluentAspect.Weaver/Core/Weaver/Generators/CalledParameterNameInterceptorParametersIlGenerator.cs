using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
   public class CalledParameterNameInterceptorParametersIlGenerator : IInterceptorParameterIlGenerator<IlInjectorAvailableVariablesForInstruction>
   {
      private readonly ParameterDefinition parameter;

      public CalledParameterNameInterceptorParametersIlGenerator(ParameterDefinition parameter)
      {
         this.parameter = parameter;
      }

      public void GenerateIl(ParameterInfo parameterInfo, List<Instruction> instructions, IlInjectorAvailableVariablesForInstruction info)
      {
         ModuleDefinition moduleDefinition = ((MethodDefinition)parameter.Method).Module;
         if (!parameterInfo.ParameterType.IsByRef && parameter.ParameterType.IsByReference)
         {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CalledParameters["called" + parameter.Name]));
            instructions.Add(Instruction.Create(OpCodes.Ldobj, moduleDefinition.Import(parameterInfo.ParameterType)));
         }
         else
         {
            instructions.Add(Instruction.Create(OpCodes.Ldloc, info.CalledParameters["called" + parameter.Name]));
         }
         if (parameter.ParameterType != moduleDefinition.TypeSystem.Object &&
             parameterInfo.ParameterType == typeof(Object))
         {
            TypeReference reference = parameter.ParameterType;
            if (reference.IsByReference)
            {
               reference =
                  ((MethodDefinition)parameter.Method).GenericParameters.First(
                     t => t.Name == reference.Name.TrimEnd('&'));
               instructions.Add(Instruction.Create(OpCodes.Ldobj, reference));
            }
            instructions.Add(Instruction.Create(OpCodes.Box, reference));
         }
      }
   }
}