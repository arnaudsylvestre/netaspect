using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Core;
using NetAspect.Weaver.Core.Weaver.Call;
using NetAspect.Weaver.Helpers;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Method
{






    public class IlInjectorAvailableVariablesForInstruction : IlInstructionInjectorAvailableVariables
    {
        private readonly MethodDefinition method;
        private readonly NetAspectWeavingMethod _netAspectWeavingMethod;
        
        private IlInjectorAvailableVariables methodVariables;

        private List<Instruction> calledInstructions = new List<Instruction>();
        private List<Instruction> calledParametersInstructions = new List<Instruction>();
        private List<Instruction> beforeAfter = new List<Instruction>();

        private List<VariableDefinition> variables = new List<VariableDefinition>();
        private VariableDefinition _called;
        private Instruction instruction;
        private Dictionary<string, VariableDefinition> _calledParameters;


        public IlInjectorAvailableVariablesForInstruction(VariableDefinition result, MethodDefinition method, NetAspectWeavingMethod netAspectWeavingMethod, IlInjectorAvailableVariables methodVariables, Instruction instruction)
        {
            this.method = method;
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
                _calledParameters = new Dictionary<string, VariableDefinition>();
                var calledMethod = instruction.GetCalledMethod();
                foreach (var parameter in calledMethod.Parameters.Reverse())
                {
                    var variableDefinition = new VariableDefinition(parameter.ParameterType);
                    _calledParameters.Add("called" + parameter.Name, variableDefinition);
                    initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
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
                    var calledMethod = instruction.GetCalledMethod();

                    _called = new VariableDefinition(calledMethod.DeclaringType);
                    _netAspectWeavingMethod.Variables.Add(_called);

                    initInstructions.Add(Instruction.Create(OpCodes.Stloc, variableDefinition));
                    recallInstructions.Add(Instruction.Create(OpCodes.Ldloc, variableDefinition));
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
        private readonly NetAspectWeavingMethod _netAspectWeavingMethod;
        private VariableDefinition _exception;
        private VariableDefinition _parameters;
        private VariableDefinition currentMethodInfo;
        private VariableDefinition currentPropertyInfo;
        private VariableDefinition _field;

        public IlInjectorAvailableVariables(VariableDefinition result, MethodDefinition method, NetAspectWeavingMethod netAspectWeavingMethod)
        {
            _result = result;
            this.method = method;
            _netAspectWeavingMethod = netAspectWeavingMethod;
        }


        public VariableDefinition CurrentMethodBase
        {
            get
            {
                if (currentMethodInfo == null)
                {
                    currentMethodInfo = new VariableDefinition(method.Module.Import(typeof(MethodBase)));
                    _netAspectWeavingMethod.Variables.Add(currentMethodInfo);

                    _netAspectWeavingMethod.BeforeInstructions.Add(Instruction.Create(OpCodes.Call,
                                                        method.Module.Import(
                                                            typeof (MethodBase).GetMethod("GetCurrentMethod",
                                                                                          new Type[] {}))));
                    _netAspectWeavingMethod.BeforeInstructions.AppendSaveResultTo(currentMethodInfo);
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
                    _parameters = new VariableDefinition(method.Module.Import(typeof(MethodBase)));
                    _netAspectWeavingMethod.Variables.Add(_parameters);

                    new NetAspect.Weaver.Helpers.IL.Method(method).FillArgsArrayFromParameters(_netAspectWeavingMethod.BeforeInstructions, _parameters);
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
                    _netAspectWeavingMethod.Variables.Add(_exception);
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
                    _netAspectWeavingMethod.Variables.Add(currentPropertyInfo);

                    _netAspectWeavingMethod.BeforeInstructions.AppendCallToThisGetType(method.Module);
                    _netAspectWeavingMethod.BeforeInstructions.AppendCallToGetProperty(method.Name.Replace("get_", "").Replace("set_", ""),
                                                         method.Module);
                    _netAspectWeavingMethod.BeforeInstructions.AppendSaveResultTo(currentPropertyInfo);
                }
                return currentPropertyInfo;
            }
        }

        
        public VariableDefinition Field { get
        {
            if (_field == null)
            {

                _field = new VariableDefinition(method.Module.Import(typeof(FieldInfo)));
                _netAspectWeavingMethod.Variables.Add(_field);
            }
            return _field;
        }}
    }
}