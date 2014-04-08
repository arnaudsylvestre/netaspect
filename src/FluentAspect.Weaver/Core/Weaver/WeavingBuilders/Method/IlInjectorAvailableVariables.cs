using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Call;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{






    public class IlInjectorAvailableVariablesForInstruction : IlInstructionInjectorAvailableVariables
    {
        private readonly NetAspectWeavingMethod _netAspectWeavingMethod;
        
        private IlInjectorAvailableVariables methodVariables;

        public List<Instruction> calledInstructions = new List<Instruction>();
        public List<Instruction> calledParametersInstructions = new List<Instruction>();
        public List<Instruction> recallcalledInstructions = new List<Instruction>();
        public List<Instruction> recallcalledParametersInstructions = new List<Instruction>();
        private List<Instruction> beforeAfter = new List<Instruction>();

        public List<VariableDefinition> Variables = new List<VariableDefinition>();
        private VariableDefinition _called;
        private Instruction instruction;
        private Dictionary<string, VariableDefinition> _calledParameters;


        public IlInjectorAvailableVariablesForInstruction(NetAspectWeavingMethod netAspectWeavingMethod, IlInjectorAvailableVariables methodVariables, Instruction instruction)
        {
            _netAspectWeavingMethod = netAspectWeavingMethod;
            this.methodVariables = methodVariables;
            this.instruction = instruction;
        }


        public VariableDefinition CurrentMethodBase
        {
            get { return methodVariables.CurrentMethodBase; }
        }

        public VariableDefinition Result
        {
            get { return methodVariables.Result; }
        }

        public VariableDefinition Parameters
        {
            get { return methodVariables.Parameters; }
        }

        public VariableDefinition Exception
        {
            get { return methodVariables.Exception; }
        }

        public VariableDefinition CurrentPropertyInfo
        {

            get { return methodVariables.CurrentPropertyInfo; }
        }

        public Dictionary<string, VariableDefinition> CalledParameters { get
        {
            if (_calledParameters == null)
            {
                if (instruction.Operand is MethodReference)
                {
                    _calledParameters = new Dictionary<string, VariableDefinition>();
                    var calledMethod = instruction.GetCalledMethod();
                    foreach (var parameter in calledMethod.Parameters.Reverse())
                    {
                        var variableDefinition = new VariableDefinition(parameter.ParameterType);
                        _calledParameters.Add("called" + parameter.Name, variableDefinition);
                        calledParametersInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
                    }
                    foreach (var parameter in calledMethod.Parameters)
                    {
                        recallcalledParametersInstructions.Add(Instruction.Create(OpCodes.Ldloc, _calledParameters["called" + parameter.Name]));
                    }
                }
                if (instruction.IsAnUpdateField())
                {
                   _calledParameters = new Dictionary<string, VariableDefinition>();
                   var fieldType = (instruction.Operand as FieldReference).Resolve().FieldType;
                   var variableDefinition = new VariableDefinition(fieldType);
                   Variables.Add(variableDefinition);
                   _calledParameters.Add("value", variableDefinition);
                   calledInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
                   recallcalledInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
                }
                
            }
            return _calledParameters;
        }}
        public VariableDefinition Called
        {
            get
            {
                if (_called == null)
                {
                    var calledParameters = CalledParameters;
                    TypeReference declaringType = null;
                    var operand = instruction.Operand as FieldReference;
                    if (operand != null && !operand.Resolve().IsStatic)
                        declaringType = operand.DeclaringType;
                    var methodReference = instruction.Operand as MethodReference;
                    if (methodReference != null && !methodReference.Resolve().IsStatic)
                        declaringType = methodReference.DeclaringType;

                    if (declaringType == null)
                        return null;

                    _called = new VariableDefinition(declaringType);
                    Variables.Add(_called);

                    calledInstructions.Add(Instruction.Create(OpCodes.Stloc, _called));
                    calledInstructions.Add(Instruction.Create(OpCodes.Ldloc, _called));
                }
                return _called;
            }
        }
        public VariableDefinition Field
        {
            get { return methodVariables.Field; }
        }
    }





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
        public List<VariableDefinition> Variables = new List<VariableDefinition>(); 

        public IlInjectorAvailableVariables(VariableDefinition result, MethodDefinition method)
        {
            _result = result;
            this.method = method;
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

                    new NetAspect.Weaver.Helpers.IL.Method(method).FillArgsArrayFromParameters(BeforeInstructions, _parameters);
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

                    BeforeInstructions.AppendCallToThisGetType(method.Module);
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