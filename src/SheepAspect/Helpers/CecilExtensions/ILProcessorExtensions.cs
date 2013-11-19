using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SheepAspect.Helpers.CecilExtensions
{
    public static class ILProcessorExtensions
    {
        public static void AppendAll(this ILProcessor il, IEnumerable<Instruction> instructions)
        {
            foreach(var i in instructions)
                il.Append(i);
        }

        public static Instruction Append(this ILProcessor il, OpCode opCode)
        {
            var i = il.Create(opCode);
            il.Append(i);
            return i;
        }
        public static Instruction Append(this ILProcessor il, OpCode opCode, ParameterDefinition arg)
        {
            var i = il.Create(opCode, arg);
            il.Append(i);
            return i;
        }
        public static Instruction Append(this ILProcessor il, OpCode opCode, MethodReference arg)
        {
            var i = il.Create(opCode, arg);
            il.Append(i);
            return i;
        }
        public static Instruction Append(this ILProcessor il, OpCode opCode, int arg)
        {
            var i = il.Create(opCode, arg);
            il.Append(i);
            return i;
        }
        public static Instruction Append(this ILProcessor il, OpCode opCode, VariableDefinition arg)
        {
            var i = il.Create(opCode, arg);
            il.Append(i);
            return i;
        }
        public static Instruction Append(this ILProcessor il, OpCode opCode, FieldReference arg)
        {
            var i = il.Create(opCode, arg);
            il.Append(i);
            return i;
        }
        public static Instruction Append(this ILProcessor il, OpCode opCode, FieldDefinition arg)
        {
            var i = il.Create(opCode, arg);
            il.Append(i);
            return i;
        }

        public static Instruction Append(this ILProcessor il, OpCode opCode, Instruction arg)
        {
            var i = il.Create(opCode, arg);
            il.Append(i);
            return i;
        }

        public static Instruction Append(this ILProcessor il, OpCode opCode, TypeReference arg)
        {
            var i = il.Create(opCode, arg);
            il.Append(i);
            return i;
        }
    }
}