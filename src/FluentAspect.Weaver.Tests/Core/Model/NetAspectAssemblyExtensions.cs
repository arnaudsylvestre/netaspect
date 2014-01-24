using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public static class NetAspectAssemblyExtensions
    {
        public static TypeDefinition AddClass(this AssemblyDefinition assembly, string name)
        {
            var typeDefinition = new TypeDefinition("A", name, TypeAttributes.Class | TypeAttributes.Public, assembly.MainModule.TypeSystem.Object);
            assembly.MainModule.Types.Add(typeDefinition);
            return typeDefinition;
        }

        public static NetAspectAspect AddAspect(this AssemblyDefinition assembly, string name)
        {
            var typeDefinition = assembly.AddClass(name);
            typeDefinition.BaseType = assembly.MainModule.Import(typeof (Attribute));
            var fieldDefinition = new FieldDefinition("NetAspectAttributeKind", FieldAttributes.Public, assembly.MainModule.TypeSystem.String);
            typeDefinition.Fields.Add(fieldDefinition);
            var method = new MethodDefinition(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, assembly.MainModule.TypeSystem.Void);
            var instructions = method.Body.Instructions;
            var constructorInfo_L = typeof(Attribute).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)[0];
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Ldstr, "MethodWeaving"));
            instructions.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition));
            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Call, assembly.MainModule.Import(constructorInfo_L)));
            instructions.Add(Instruction.Create(OpCodes.Ret));
            typeDefinition.Methods.Add(method);
            return new NetAspectAspect(typeDefinition);
        }
        public static NetAspectAspect AddDefaultAspect(this AssemblyDefinition assembly, string name)
        {
           var aspect = assembly.AddAspect(name);
           var addDefaultConstructor_L = aspect.AddDefaultConstructor();
           addDefaultConstructor_L.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
           return aspect;
        }
        public static MethodDefinition AddMethod(this TypeDefinition type, string name)
        {
           var methodDefinition_L = new MethodDefinition(name, MethodAttributes.Public, type.Module.TypeSystem.Void);
           type.Methods.Add(methodDefinition_L);
           return methodDefinition_L;
        }
        public static MethodDefinition WithReturn(this MethodDefinition method)
        {
           method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
           return method;
        }
        public static MethodDefinition WithParameter<T>(this MethodDefinition method, string parameterName)
        {
           method.Parameters.Add(new ParameterDefinition(parameterName, ParameterAttributes.None, method.Module.Import(typeof(T))));
           return method;
        }
        public static MethodDefinition WithAspect(this MethodDefinition method, NetAspectAspect aspect)
        {
           method.CustomAttributes.Add(new CustomAttribute(aspect.TypeDefinition.GetDefaultConstructor()));
           return method;
        }
        public static MethodDefinition GetDefaultConstructor(this TypeDefinition type)
        {
           return (from m in type.Methods where m.Name == ".ctor" && m.Parameters.Count == 0 select m).First();
        }
        public static TypeDefinition WithDefaultConstructor<T>(this TypeDefinition type)
        {
           var constructorInfo_L = typeof(T).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)[0];
           MethodWeavingAspectDefiner.AddEmptyConstructor(type, type.Module.Import(constructorInfo_L), new List<Instruction>());
           return type;
        }
        public static TypeDefinition WithDefaultConstructor(this TypeDefinition type)
        {
           return type.WithDefaultConstructor<object>();
        }
    }
}