using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
   public class IlInjectorAvailableVariables : IlInstructionInjectorAvailableVariables
    {
        private readonly VariableDefinition _result;
        private readonly MethodDefinition method;
        private VariableDefinition _exception;
        private VariableDefinition _parameters;
        private VariableDefinition currentMethodInfo;
        private VariableDefinition currentPropertyInfo;
        private VariableDefinition _field;

        public List<Instruction> BeforeInstructions = new List<Instruction>();

        public IlInjectorAvailableVariables(VariableDefinition result, MethodDefinition method)
        {
            _result = result;
            this.method = method;
            Variables = new List<VariableDefinition>();
            Fields = new List<FieldDefinition>();
        }


        public VariableDefinition CurrentMethodBase
        {
            get
            {
                if (currentMethodInfo == null)
                {
                    currentMethodInfo = new VariableDefinition(method.Module.Import(typeof(MethodBase)));
                    Variables.Add(currentMethodInfo);

                    BeforeInstructions.Add(Instruction.Create(OpCodes.Call,
                                                        method.Module.Import(
                                                            typeof (MethodBase).GetMethod("GetCurrentMethod",
                                                                                          new Type[] {}))));
                    BeforeInstructions.AppendSaveResultTo(currentMethodInfo);
                }
                return currentMethodInfo;
            }
        }

        public List<VariableDefinition> Variables { get; private set; }
        public List<FieldDefinition> Fields { get; private set; }

        public VariableDefinition Result
        {
            get { return _result; }
        }

        public VariableDefinition Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new VariableDefinition(method.Module.Import(typeof(object[])));
                    Variables.Add(_parameters);

                    method.FillArgsArrayFromParameters(BeforeInstructions, _parameters);
                }
                return _parameters;
            }
        }

        public VariableDefinition Exception
        {
            get
            {
                if (_exception == null)
                {
                    _exception = new VariableDefinition(method.Module.Import(typeof(Exception)));
                    Variables.Add(_exception);
                }
                return _exception;
            }
        }

        public VariableDefinition CurrentPropertyInfo
        {
            get
            {
                if (currentPropertyInfo == null)
                {
                    currentPropertyInfo = new VariableDefinition(method.Module.Import(typeof(PropertyInfo)));
                    Variables.Add(currentPropertyInfo);


                    BeforeInstructions.Add(Instruction.Create(OpCodes.Call,
                                                        method.Module.Import(
                                                            typeof(MethodBase).GetMethod("GetCurrentMethod",
                                                                                          new Type[] { }))));
                    BeforeInstructions.Add(Instruction.Create(OpCodes.Callvirt,
                                                        method.Module.Import(
                                                            typeof(MemberInfo).GetMethod("get_DeclaringType",
                                                                                          new Type[] { }))));
                    BeforeInstructions.AppendCallToGetProperty(method.Name.Replace("get_", "").Replace("set_", ""),
                                                         method.Module);
                    BeforeInstructions.AppendSaveResultTo(currentPropertyInfo);
                }
                return currentPropertyInfo;
            }
        }

        
        public VariableDefinition Field { get
        {
            if (_field == null)
            {

                _field = new VariableDefinition(method.Module.Import(typeof(FieldInfo)));
                Variables.Add(_field);
            }
            return _field;
        }}
    }
}