using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectAspect
    {
        private readonly TypeDefinition typeDefinition;

        public NetAspectAspect(TypeDefinition typeDefinition)
        {
            this.typeDefinition = typeDefinition;
        }

        public MethodDefinition AddDefaultConstructor()
        {
            var constructor = new MethodDefinition(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, typeDefinition.Module.TypeSystem.Void);

            var instructions = constructor.Body.Instructions;

            var parentConstructor = (from m in typeDefinition.BaseType.Resolve().Methods where m.Name == ".ctor" && m.Parameters.Count == 0 select m).First();

            instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Call, parentConstructor));
            instructions.Add(Instruction.Create(OpCodes.Ret));

            return constructor;
        }

        public NetAspectInterceptor AddAfterInterceptor()
        {
            var interceptor = new MethodDefinition("After", MethodAttributes.Public, typeDefinition.Module.TypeSystem.Void);

            return new NetAspectInterceptor(interceptor);
        }
    }

    public class NetAspectInterceptor
    {
        private MethodDefinition definition;

        public NetAspectInterceptor(MethodDefinition definition)
        {
            this.definition = definition;
        }


        public NetAspectInterceptor WithParameter<T>(string parameterName)
        {
            var netAspectParameter = new ParameterDefinition(parameterName, ParameterAttributes.None,
                                                             definition.Module.Import(typeof (T)));
            definition.Parameters.Add(netAspectParameter);
            var netAspectField = new FieldDefinition(definition.Name + parameterName + "Field", FieldAttributes.Public, definition.Module.Import(typeof(T)))
                {
                    IsStatic = true
                };
            definition.DeclaringType.Fields.Add(netAspectField);

            if (!fieldDefinition.IsStatic)
                instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Ldarg, parameterDefinition));

            if (parameterDefinition.ParameterType.IsByReference)
                instructions.Add(Instruction.Create(OpCodes.Ldind_Ref));
            instructions.Add(Instruction.Create(fieldDefinition.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldDefinition));
            _netAspectClass.Add(netAspectField);
            method.AddInstruction(new AffectFieldWithParameterInstruction(netAspectField, netAspectParameter));
            return this;
        }

        public NetAspectInterceptor WithReturn()
        {
            method.WithReturn();
            return this;
        }

        public NetAspectInterceptor WithReferencedParameter<T>(string parameterName)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, new ByReferenceType(method.ModuleDefinition.Import(typeof(T))), false);
            method.Add(netAspectParameter);
            var netAspectField = new NetAspectField(method.Name + parameterName + "Field", method.ModuleDefinition.Import(typeof(T)))
            {
                IsStatic = true
            };
            _netAspectClass.Add(netAspectField);
            method.AddInstruction(new AffectFieldWithParameterInstruction(netAspectField, netAspectParameter));
            return this;
        }

        public NetAspectInterceptor WithOutParameter<T>(string parameterName)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, new ByReferenceType(method.ModuleDefinition.Import(typeof(T))), true);
            method.Add(netAspectParameter);
            var netAspectField = new NetAspectField(method.Name + parameterName + "Field", method.ModuleDefinition.Import(typeof(T)))
            {
                IsStatic = true
            };
            _netAspectClass.Add(netAspectField);
            method.AddInstruction(new AffectFieldWithParameterInstruction(netAspectField, netAspectParameter));
            return this;
        }

        public NetAspectInterceptor WithParameter(string parameterName, NetAspectClass myClassToWeave)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, myClassToWeave, false);
            method.Add(netAspectParameter);
            var netAspectField = new NetAspectField(method.Name + parameterName + "Field", myClassToWeave)
            {
                IsStatic = true
            };
            _netAspectClass.Add(netAspectField);
            method.AddInstruction(new AffectFieldWithParameterInstruction(netAspectField, netAspectParameter));
            return this;
        }
    }

    public class AffectFieldWithParameterInstruction : INetAspectMethodInstruction
    {
        private readonly NetAspectField _netAspectField;
        private readonly NetAspectParameter _netAspectParameter;

        public AffectFieldWithParameterInstruction(NetAspectField netAspectField, NetAspectParameter netAspectParameter)
        {
            _netAspectField = netAspectField;
            _netAspectParameter = netAspectParameter;
        }

        public void Generate(Collection<Instruction> instructions)
        {
            var parameterDefinition = _netAspectParameter.ParameterDefinition;
            var fieldDefinition = _netAspectField.Field;
            if (!fieldDefinition.IsStatic)
                instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            instructions.Add(Instruction.Create(OpCodes.Ldarg, parameterDefinition));

            if (parameterDefinition.ParameterType.IsByReference)
                instructions.Add(Instruction.Create(OpCodes.Ldind_Ref));
            instructions.Add(Instruction.Create(fieldDefinition.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldDefinition));
        }
    }
}