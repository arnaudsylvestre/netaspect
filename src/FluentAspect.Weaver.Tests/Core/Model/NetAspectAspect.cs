using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace FluentAspect.Weaver.Tests.Core.Model
{
    public class NetAspectAspect
    {
        private readonly NetAspectClass _netAspectClass;

        public NetAspectAspect(NetAspectClass netAspectClass)
        {
            _netAspectClass = netAspectClass;
        }

        public NetAspectMethod AddDefaultConstructor()
        {
            return _netAspectClass.AddConstructor();
        }

        public MethodReference DefaultConstructor
        {
            get { return _netAspectClass.DefaultConstructor; }
        }

        public NetAspectInterceptor AddAfterInterceptor()
        {
            return new NetAspectInterceptor(_netAspectClass.AddMethod("After"), _netAspectClass);
        }
    }

    public class NetAspectInterceptor
    {
        private NetAspectMethod method;
        private readonly NetAspectClass _netAspectClass;

        public NetAspectInterceptor(NetAspectMethod method, NetAspectClass netAspectClass)
        {
            this.method = method;
            _netAspectClass = netAspectClass;
        }


        public NetAspectParameter WithParameter<T>(string parameterName)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, method.ModuleDefinition.Import(typeof(T)), false);
            method.Add(netAspectParameter);
            var netAspectField = new NetAspectField(method.Name + parameterName + "Field", method.ModuleDefinition.Import(typeof (T)))
                {
                    IsStatic = true
                };
            _netAspectClass.Add(netAspectField);
            method.AddInstruction(new AffectFieldWithParameterInstruction(netAspectField, netAspectParameter));
            return netAspectParameter;
        }

        public NetAspectInterceptor WithReturn()
        {
            method.WithReturn();
            return this;
        }

        public NetAspectParameter WithReferencedParameter<T>(string parameterName)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, new ByReferenceType(method.ModuleDefinition.Import(typeof(T))), false);
            method.Add(netAspectParameter);
            var netAspectField = new NetAspectField(method.Name + parameterName + "Field", method.ModuleDefinition.Import(typeof(T)))
            {
                IsStatic = true
            };
            _netAspectClass.Add(netAspectField);
            method.AddInstruction(new AffectFieldWithParameterInstruction(netAspectField, netAspectParameter));
            return netAspectParameter;
        }

        public NetAspectParameter WithOutParameter<T>(string parameterName)
        {
            var netAspectParameter = new NetAspectParameter(parameterName, new ByReferenceType(method.ModuleDefinition.Import(typeof(T))), true);
            method.Add(netAspectParameter);
            var netAspectField = new NetAspectField(method.Name + parameterName + "Field", method.ModuleDefinition.Import(typeof(T)))
            {
                IsStatic = true
            };
            _netAspectClass.Add(netAspectField);
            method.AddInstruction(new AffectFieldWithParameterInstruction(netAspectField, netAspectParameter));
            return netAspectParameter;
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
            instructions.Add(Instruction.Create(fieldDefinition.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldDefinition));
        }
    }
}