using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace FluentAspect.Weaver.Tests.Core
{
    public class MethodWeavingAspectDefiner
    {
        public static MethodDefinition AddEmptyConstructor(TypeDefinition type, MethodReference baseEmptyConstructor, IEnumerable<Instruction> instructions)
        {
            var methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
            var method = new MethodDefinition(".ctor", methodAttributes, type.Module.TypeSystem.Void);
            foreach (var instruction in instructions)
            {
                method.Body.Instructions.Add(instruction);
            }
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Call, baseEmptyConstructor));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            type.Methods.Add(method);
            return method;
        }
    }
}