using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using EventAttributes = Mono.Cecil.EventAttributes;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;
using PropertyAttributes = Mono.Cecil.PropertyAttributes;
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
            var fieldDefinition = new FieldDefinition("NetAspectAttribute", FieldAttributes.Public, assembly.MainModule.TypeSystem.String);
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
        public static EventDefinition AddEvent(this TypeDefinition type, string name, TypeReference eventType)
        {
            var eventDefinition = new EventDefinition(name, EventAttributes.None, eventType);
            var fieldDefinition = new FieldDefinition(name, FieldAttributes.Public, eventType);
            AddSubcsribeEvent(type, name, eventType, eventDefinition, fieldDefinition);
            AddRemoveEvent(type, name, eventType, eventDefinition, fieldDefinition);
            type.Events.Add(eventDefinition);
            type.Fields.Add(fieldDefinition);
            return eventDefinition;
        }
        public static EventDefinition AddEvent<T>(this TypeDefinition type, string name)
        {
            return type.AddEvent(name, type.Module.Import(typeof (T)));
        }

        public static MethodDefinition WhichCallEvent<T>(this MethodDefinition method, EventDefinition evt)
        {
            var fieldDefinition = evt.DeclaringType.Fields.First(f => f.Name == evt.Name);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, fieldDefinition));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Callvirt, method.Module.Import(typeof(T).GetMethod("Invoke"))));
            return method;
        }

        private static void AddSubcsribeEvent(TypeDefinition type, string name, TypeReference eventType,
                                              EventDefinition eventDefinition, FieldDefinition fieldDefinition)
        {
            eventDefinition.AddMethod = type.AddMethod("add_" + name);
            eventDefinition.AddMethod.Parameters.Add(new ParameterDefinition(eventType));
            eventDefinition.AddMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            eventDefinition.AddMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            eventDefinition.AddMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, fieldDefinition));
            eventDefinition.AddMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            eventDefinition.AddMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Call,
                                                                               type.Module.Import(
                                                                                   typeof(Delegate).GetMethod("Combine",
                                                                                                               new Type[]
                                                                                                                   {
                                                                                                                       typeof (
                                                                                                                   Delegate),
                                                                                                                       typeof (
                                                                                                                   Delegate)
                                                                                                                   }))));
            eventDefinition.AddMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Castclass, eventType));
            eventDefinition.AddMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition));
            eventDefinition.AddMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
        }
        private static void AddRemoveEvent(TypeDefinition type, string name, TypeReference eventType,
                                             EventDefinition eventDefinition, FieldDefinition fieldDefinition)
        {
            eventDefinition.RemoveMethod = type.AddMethod("remove_" + name);
            eventDefinition.RemoveMethod.Parameters.Add(new ParameterDefinition(eventType));
            eventDefinition.RemoveMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            eventDefinition.RemoveMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            eventDefinition.RemoveMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, fieldDefinition));
            eventDefinition.RemoveMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_1));
            eventDefinition.RemoveMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Call,
                                                                               type.Module.Import(
                                                                                   typeof(Delegate).GetMethod("Remove",
                                                                                                               new Type[]
                                                                                                                   {
                                                                                                                       typeof (
                                                                                                                   Delegate),
                                                                                                                       typeof (
                                                                                                                   Delegate)
                                                                                                                   }))));
            eventDefinition.RemoveMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Castclass, eventType));
            eventDefinition.RemoveMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Stfld, fieldDefinition));
            eventDefinition.RemoveMethod.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
        }

        public static PropertyDefinition AddProperty<T>(this TypeDefinition type, string name)
        {
            var methodDefinition_L = new PropertyDefinition(name, PropertyAttributes.None, type.Module.Import(typeof(T)));
            type.Properties.Add(methodDefinition_L);
            return methodDefinition_L;
        }
        public static MethodDefinition AddMethod<T>(this TypeDefinition type, string name)
        {
            var methodDefinition_L = new MethodDefinition(name, MethodAttributes.Public, type.Module.Import(typeof(T)));
            type.Methods.Add(methodDefinition_L);
            return methodDefinition_L;
        }
        public static MethodDefinition WithReturn(this MethodDefinition method)
        {
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return method;
        }
        public static MethodDefinition WithReturn(this MethodDefinition method, string stringValue)
        {
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldstr, stringValue));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return method;
        }
        public static MethodDefinition WithGenericType(this MethodDefinition method, string typeName)
        {
            method.GenericParameters.Add(new GenericParameter(typeName, method));
            return method;
        }
        public static MethodDefinition WithGenericParameter(this MethodDefinition method, string parameterName, string typeName)
        {
            method.Parameters.Add(new ParameterDefinition(parameterName, ParameterAttributes.None, method.GenericParameters.First(p => p.Name == typeName)));
            return method;
        }
        public static MethodDefinition WithReferencedGenericParameter(this MethodDefinition method, string parameterName, string typeName)
        {
            method.Parameters.Add(new ParameterDefinition(parameterName, ParameterAttributes.None, new ByReferenceType(method.GenericParameters.First(p => p.Name == typeName))));
            return method;
        }
        public static MethodDefinition WithReturnParameter(this MethodDefinition method, string parameterName)
        {
            var parameterDefinition = method.Parameters.First(p => p.Name == parameterName);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg, parameterDefinition));
            if (parameterDefinition.ParameterType.IsByReference)
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldind_Ref));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            return method;
        }
        public static MethodDefinition WithParameter<T>(this MethodDefinition method, string parameterName)
        {
            method.Parameters.Add(new ParameterDefinition(parameterName, ParameterAttributes.None, method.Module.Import(typeof(T))));
            return method;
        }
        public static MethodDefinition WithParameter<T>(this MethodDefinition method, string parameterName, NetAspectAspect aspect)
        {
            var parameterDefinition = new ParameterDefinition(parameterName, ParameterAttributes.None, method.Module.Import(typeof (T)));
            parameterDefinition.CustomAttributes.Add(new CustomAttribute(aspect.TypeDefinition.GetDefaultConstructor()));
            method.Parameters.Add(parameterDefinition);
            return method;
        }
        public static MethodDefinition WithReferencedParameter<T>(this MethodDefinition method, string parameterName)
        {
            method.Parameters.Add(new ParameterDefinition(parameterName, ParameterAttributes.None, new ByReferenceType(method.Module.Import(typeof(T)))));
            return method;
        }


        public static MethodDefinition WithCallMethodWithParameter(this MethodDefinition method, string methodToCall, string parameterName)
        {
            var methodDefinition = method.DeclaringType.Methods.First(m => m.Name == methodToCall);
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            if (methodDefinition.Parameters[0].ParameterType.IsByReference)
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarga,
                                                                method.Parameters.First(p => p.Name == parameterName)));
            else
                method.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg,
                                                                method.Parameters.First(p => p.Name == parameterName)));
            method.Body.Instructions.Add(Instruction.Create(OpCodes.Call, methodDefinition));
            return method;
        }

        public static MethodDefinition WithAspect(this MethodDefinition method, NetAspectAspect aspect)
        {
            method.CustomAttributes.Add(new CustomAttribute(aspect.TypeDefinition.GetDefaultConstructor()));
            return method;
        }

        public static EventDefinition WithAspect(this EventDefinition method, NetAspectAspect aspect)
        {
            method.CustomAttributes.Add(new CustomAttribute(aspect.TypeDefinition.GetDefaultConstructor()));
            return method;
        }

        public static PropertyDefinition WithAspect(this PropertyDefinition method, NetAspectAspect aspect)
        {
            method.CustomAttributes.Add(new CustomAttribute(aspect.TypeDefinition.GetDefaultConstructor()));
            return method;
        }

        public static MethodDefinition AddGetMethod(this PropertyDefinition property)
        {
            property.GetMethod = new MethodDefinition("get_" + property.Name, MethodAttributes.Public |
MethodAttributes.SpecialName |
MethodAttributes.HideBySig, property.PropertyType);
            property.DeclaringType.Methods.Add(property.GetMethod);
            return property.GetMethod;
        }
        public static MethodDefinition AddSetMethod(this PropertyDefinition property)
        {
            property.SetMethod = new MethodDefinition("set_" + property.Name, MethodAttributes.Public |
MethodAttributes.SpecialName |
MethodAttributes.HideBySig, property.Module.TypeSystem.Void);
            property.DeclaringType.Methods.Add(property.SetMethod);
            property.SetMethod.Parameters.Add(new ParameterDefinition("value", ParameterAttributes.None, property.PropertyType));
            return property.SetMethod;
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