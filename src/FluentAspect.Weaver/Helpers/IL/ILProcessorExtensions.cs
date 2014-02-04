using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Helpers.IL
{
    public static class ILProcessorExtensions
    {
        public static void AppendThrow(this Collection<Instruction> instructions)
        {
            instructions.Add(Instruction.Create(OpCodes.Rethrow));
        }

        public static void AppendCallToThisGetType(this Collection<Instruction> instructions, ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Call, module.Import(typeof (object).GetMethod("GetType", new Type[0]))));
        }

        public static void AppendCallToGetMethod(this Collection<Instruction> instructions, string methodName, ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldstr, methodName));
            instructions.Add(Instruction.Create(OpCodes.Callvirt, module.Import(typeof (Type).GetMethod("GetMethod", new[] {typeof (string)}))));
        }

        public static void AppendSaveResultTo(this Collection<Instruction> instructions, VariableDefinition variable)
        {
            instructions.Add(Instruction.Create(OpCodes.Stloc, variable));
        }

        public static Instruction AppendLeave(this List<Instruction> instructions, Instruction instruction_P)
        {
            Instruction instruction_L = Instruction.Create(OpCodes.Leave, instruction_P);
            instructions.Add(instruction_L);
            return instruction_L;
        }

        public static void AppendCreateNewObject(this Collection<Instruction> instructions,
                                                 VariableDefinition interceptor,
                                                 Type interceptorType,
                                                 ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Newobj, module.Import(interceptorType.GetConstructors()[0])));
            instructions.Add(Instruction.Create(OpCodes.Stloc, interceptor));
        }


        public static VariableDefinition CreateAndInitializeVariable(this Collection<Instruction> instructions, MethodDefinition method,
                                                                     Type type)
        {
            VariableDefinition variable_L = method.CreateVariable(type);
            instructions.AppendCreateNewObject(variable_L, type, method.Module);
            return variable_L;
        }
    }
}