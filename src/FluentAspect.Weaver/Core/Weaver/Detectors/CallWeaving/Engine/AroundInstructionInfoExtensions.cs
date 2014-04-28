using System;
using System.Collections.Generic;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public static class AroundInstructionInfoExtensions
    {
        public static FieldDefinition GetOperandAsField(this AroundInstructionInfo aroundInstruction)
        {
            return (aroundInstruction.Instruction.Operand as FieldReference).Resolve();
        }
        public static MethodDefinition GetOperandAsMethod(this AroundInstructionInfo aroundInstruction)
        {
            return (aroundInstruction.Instruction.Operand as MethodReference).Resolve();
        }

        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo> AddPossibleParameter(this AroundInstructionInfo aroundInstruction,
                                                                                                                        string parameterName)
        {
            var myGenerator = new MyGenerator<IlInjectorAvailableVariablesForInstruction>();
            var checker = new MyInterceptorParameterChecker();
            aroundInstruction.Generator.Add(parameterName, myGenerator, checker);
            return new AroundInstructionParametersConfigurator<IlInjectorAvailableVariablesForInstruction, AroundInstructionInfo>(myGenerator, checker, aroundInstruction);
        }
        public static AroundInstructionParametersConfigurator<IlInjectorAvailableVariables, AroundMethodInfo> AddPossibleParameter(this AroundMethodInfo aroundInstruction,
                                                                                                                        string parameterName)
        {
            var myGenerator = new MyGenerator<IlInjectorAvailableVariables>();
            var checker = new MyInterceptorParameterChecker();
            aroundInstruction.Generator.Add(parameterName, myGenerator, checker);
            return new AroundInstructionParametersConfigurator<IlInjectorAvailableVariables, AroundMethodInfo>(myGenerator, checker, aroundInstruction);
        }

        
    }
}