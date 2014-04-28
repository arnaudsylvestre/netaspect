using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public static class AroundInstructionParametersConfiguratorExtensions
    {
        private List<string> allowedTypes = new List<string>();

        public AroundInstructionParametersConfigurator<T, TAroundInfo> WhichCanNotBeReferenced()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotReferenced(parameter, errorListener));
            return this;
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> WhichPdbPresent()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.SequencePoint(_aroundInstruction.Instruction, errorListener, parameter));
            return this;
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> WhichCanNotBeOut()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotOut(parameter, errorListener));
            return this;
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> WhereFieldCanNotBeStatic()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotStaticButDefaultValue(parameter, errorListener, _aroundInstruction.GetOperandAsField()));
            return this;
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> WhereCurrentMethodCanNotBeStatic()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotStatic(parameter, errorListener, _aroundInstruction.Method));
            return this;
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> WhichMustBeOfType<T1>()
        {
            allowedTypes.Add(typeof(T1).FullName);
            return this;
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> WhichMustBeOfTypeOfParameter(ParameterDefinition parameterDefinition)
        {
            _checker.Checkers.Add((info, handler) => Ensure.OfType(info, handler, parameterDefinition));
            return this;
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> OrOfType(TypeReference type)
        {
            return WhichMustBeOfTypeOf(type);
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> WhichMustBeOfTypeOf(TypeReference type)
        {
            allowedTypes.Add(type.FullName);
            return this;
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> OrOfFieldDeclaringType()
        {
            return OrOfType(_aroundInstruction.GetOperandAsField().DeclaringType);
        }

        public AroundInstructionParametersConfigurator<T, TAroundInfo> OrOfCurrentMethodDeclaringType()
        {
            return OrOfType(_aroundInstruction.Method.DeclaringType);
        }


        public AroundInstructionParametersConfigurator<T, TAroundInfo> AndInjectThePdbInfo(Func<SequencePoint, int> pdbInfoProvider)
        {
            _myGenerator.Generators.Add((parameter, instructions, info) =>
                {
                    SequencePoint instructionPP = _aroundInstruction.Instruction.GetLastSequencePoint();
                    instructions.Add(Instruction.Create(OpCodes.Ldc_I4,
                                                        instructionPP == null
                                                            ? 0
                                                            : pdbInfoProvider(instructionPP)));
                });
            return this;
        }
        public AroundInstructionParametersConfigurator<T, TAroundInfo> AndInjectThePdbInfo(Func<SequencePoint, string> pdbInfoProvider)
        {
            _myGenerator.Generators.Add((parameter, instructions, info) =>
                {
                    SequencePoint instructionPP = _aroundInstruction.Instruction.GetLastSequencePoint();
                    instructions.Add(Instruction.Create(OpCodes.Ldstr,
                                                        instructionPP == null
                                                            ? null
                                                            : pdbInfoProvider(instructionPP)));
                });
            return this;
        }

        
    }
}