using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Call;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Method
{
    public class IlInjectorAvailableVariables : IlInstructionInjectorAvailableVariables
    {
        private readonly VariableDefinition _result;
        private readonly MethodDefinition method;
        public List<Instruction> Instructions = new List<Instruction>();
        private VariableDefinition _exception;
        private VariableDefinition _parameters;
        private VariableDefinition currentMethodInfo;
        private VariableDefinition currentPropertyInfo;
        private VariableDefinition _field;

        public IlInjectorAvailableVariables(VariableDefinition result, MethodDefinition method)
        {
            _result = result;
            this.method = method;
            VariablesByInstruction = new Dictionary<Instruction, Dictionary<string, VariableDefinition>>();
            VariablesCalled = new Dictionary<Instruction, VariableDefinition>();
        }


        public VariableDefinition CurrentMethodBase
        {
            get
            {
                if (currentMethodInfo == null)
                {
                    currentMethodInfo = method.CreateVariable<MethodBase>();

                    Instructions.Add(Instruction.Create(OpCodes.Call,
                                                        method.Module.Import(
                                                            typeof (MethodBase).GetMethod("GetCurrentMethod",
                                                                                          new Type[] {}))));
                    Instructions.AppendSaveResultTo(currentMethodInfo);
                }
                return currentMethodInfo;
            }
        }

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
                    _parameters = method.CreateVariable<object[]>();

                    new NetAspect.Weaver.Helpers.IL.Method(method).FillArgsArrayFromParameters(Instructions, _parameters);
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
                    _exception = method.CreateVariable<Exception>();
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
                    currentPropertyInfo = method.CreateVariable<PropertyInfo>();

                    Instructions.AppendCallToThisGetType(method.Module);
                    Instructions.AppendCallToGetProperty(method.Name.Replace("get_", "").Replace("set_", ""),
                                                         method.Module);
                    Instructions.AppendSaveResultTo(currentPropertyInfo);
                }
                return currentPropertyInfo;
            }
        }

        public Dictionary<Instruction, Dictionary<string, VariableDefinition>> VariablesByInstruction { get; private set; }
        public Dictionary<Instruction, VariableDefinition> VariablesCalled { get; private set; }
        public VariableDefinition Field { get
        {
            if (_field == null)
            {
                _field = method.CreateVariable<FieldInfo>();
            }
            return _field;
        }}
    }
}