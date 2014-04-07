using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace NetAspect.Weaver.Helpers.IL
{
    public static class InstructionsExtensions
    {
        public static void AppendCallToThisGetType(this List<Instruction> instructions, ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Call,
                                                module.Import(typeof (object).GetMethod("GetType", new Type[0]))));
        }

        public static void AppendCallToGetMethod(this List<Instruction> instructions, string methodName,
                                                 ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldstr, methodName));
            instructions.Add(Instruction.Create(OpCodes.Callvirt,
                                                module.Import(typeof(Type).GetMethod("GetMethod",
                                                                                      new[] { typeof(string) }))));
        }

        public static void AppendCallToGetField(this List<Instruction> instructions, string fieldName,
                                                 ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldstr, fieldName));
            instructions.Add(Instruction.Create(OpCodes.Callvirt,
                                                module.Import(typeof(Type).GetMethod("GetField",
                                                                                      new[] { typeof(string) }))));
        }

        public static void AppendCallToGetProperty(this List<Instruction> instructions, string propertyName,
                                                   ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Ldstr, propertyName));
            instructions.Add(Instruction.Create(OpCodes.Callvirt,
                                                module.Import(typeof (Type).GetMethod("GetProperty",
                                                                                      new[] {typeof (string)}))));
        }

        public static void AppendSaveResultTo(this List<Instruction> instructions, VariableDefinition variable)
        {
            instructions.Add(Instruction.Create(OpCodes.Stloc, variable));
        }

       public static void AppendCreateNewObject(this List<Instruction> instructions,
                                                 VariableDefinition interceptor,
                                                 Type interceptorType,
                                                 ModuleDefinition module)
        {
            instructions.Add(Instruction.Create(OpCodes.Newobj, module.Import(interceptorType.GetConstructors()[0])));
            instructions.Add(Instruction.Create(OpCodes.Stloc, interceptor));
        }
    }
}