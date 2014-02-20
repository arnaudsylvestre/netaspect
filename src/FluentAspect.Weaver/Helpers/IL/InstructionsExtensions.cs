using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Helpers.IL
{
    public static class InstructionsExtensions
    {
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

        public static void AppendCreateNewObject(this Collection<Instruction> instructions,
                                                 VariableDefinition interceptor,
                                                 Type interceptorType,
                                                 ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Newobj, module.Import(interceptorType.GetConstructors()[0])));
            instructions.Add(Instruction.Create(OpCodes.Stloc, interceptor));
        }

        public static void AppendCreateNewObject(this List<Instruction> instructions,
                                                 VariableDefinition interceptor,
                                                 Type interceptorType,
                                                 ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Newobj, module.Import(interceptorType.GetConstructors()[0])));
            instructions.Add(Instruction.Create(OpCodes.Stloc, interceptor));
        }


        public static void InitializeInterceptors(this Collection<Instruction> instructions, MethodDefinition method,
                                                                     Type type, VariableDefinition variable)
        {
            instructions.AppendCreateNewObject(variable, type, method.Module);
        }
    }
}