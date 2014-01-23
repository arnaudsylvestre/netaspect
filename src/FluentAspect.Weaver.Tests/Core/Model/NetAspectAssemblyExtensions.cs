using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
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
           aspect.AddDefaultConstructor().WithReturn();
           return aspect;
        }
        public static NetAspectMethod AddMethod(this NetAspectClass classe, string methodName)
        {
           var c = new NetAspectMethod(methodName, classe.Module.TypeSystem.Void, classe.Module, classe);
            classe.Add(c);
            return c;
        }
        public static NetAspectMethod AddConstructor(this NetAspectClass classe)
        {
           var c = new NetAspectMethod(".ctor", classe.Module.TypeSystem.Void, classe.Module, classe);
            classe.Add(c);
            return c;
        }

        public static NetAspectClass WithDefaultConstructor(this NetAspectClass classe)
        {
            classe.AddConstructor().WithReturn();
            return classe;
        }


        public static NetAspectParameter WithReferencedParameter<T>(this NetAspectMethod method, string parameterName)
        {
           var netAspectParameter = new NetAspectParameter(parameterName, new ByReferenceType(method.ModuleDefinition.Import(typeof(T))), false);
           method.Add(netAspectParameter);
           return netAspectParameter;
        }



        public static NetAspectParameter WithOutParameter<T>(this NetAspectMethod method, string parameterName)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, new ByReferenceType(method.ModuleDefinition.Import(typeof(T))), true);
            method.Add(netAspectParameter);
            return netAspectParameter;
        }

        public static NetAspectParameter WithParameter<T>(this NetAspectMethod method, string parameterName)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, method.ModuleDefinition.Import(typeof(T)), false);
            method.Add(netAspectParameter);
            return netAspectParameter;
        }


        public static NetAspectMethod WithReturn(this NetAspectMethod method)
        {
           method.AddInstruction(new ReturnInstruction());
           return method;
        }
    }
}