using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public class InterceptorParameterConfigurator<T>
    {
        private readonly InterceptorInfoExtensions.MyGenerator<T> _myGenerator;
        private readonly InterceptorInfoExtensions.MyInterceptorParameterChecker _checker;
        private readonly InterceptorInfo _interceptor;

        private List<string> allowedTypes = new List<string>();

        public InterceptorInfoExtensions.MyGenerator<T> Generator
        {
            get { return _myGenerator; }
        }

        public InterceptorInfo Interceptor
        {
            get { return _interceptor; }
        }

        public InterceptorParameterConfigurator(InterceptorInfoExtensions.MyGenerator<T> myGenerator, InterceptorInfoExtensions.MyInterceptorParameterChecker checker, InterceptorInfo interceptor)
        {
            _myGenerator = myGenerator;
            _checker = checker;
            _interceptor = interceptor;
            _checker.Checkers.Add((info, handler) => Ensure.OfType(info, handler, allowedTypes.ToArray()));
        }

        public InterceptorParameterConfigurator<T> WhichCanNotBeReferenced()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotReferenced(parameter, errorListener));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhichPdbPresent()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.SequencePoint(_interceptor.Instruction, errorListener, parameter));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhichCanNotBeOut()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotOut(parameter, errorListener));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhereFieldCanNotBeStatic()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotStaticButDefaultValue(parameter, errorListener, _interceptor.GetOperandAsField()));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhereCurrentMethodCanNotBeStatic()
        {
            _checker.Checkers.Add((parameter, errorListener) => Ensure.NotStatic(parameter, errorListener, _interceptor.Method));
            return this;
        }

        public InterceptorParameterConfigurator<T> WhichMustBeOfType<T1>()
        {
            allowedTypes.Add(typeof(T1).FullName);
            return this;
        }

        public InterceptorParameterConfigurator<T> WhichMustBeOfTypeOfParameter(ParameterDefinition parameterDefinition)
        {
            _checker.Checkers.Add((info, handler) => Ensure.OfType(info, handler, parameterDefinition));
            return this;
        }

        public InterceptorParameterConfigurator<T> OrOfType(TypeReference type)
        {
            return WhichMustBeOfTypeOf(type);
        }

        public InterceptorParameterConfigurator<T> WhichMustBeOfTypeOf(TypeReference type)
        {
            allowedTypes.Add(type.FullName);
            return this;
        }

        public InterceptorParameterConfigurator<T> OrOfFieldDeclaringType()
        {
            return OrOfType(_interceptor.GetOperandAsField().DeclaringType);
        }

        public InterceptorParameterConfigurator<T> OrOfCurrentMethodDeclaringType()
        {
            return OrOfType(_interceptor.Method.DeclaringType);
        }


        public InterceptorParameterConfigurator<T> AndInjectThePdbInfo(Func<SequencePoint, int> pdbInfoProvider)
        {
            _myGenerator.Generators.Add((parameter, instructions, info) =>
                {
                    SequencePoint instructionPP = _interceptor.Instruction.GetLastSequencePoint();
                    instructions.Add(Instruction.Create(OpCodes.Ldc_I4,
                                                        instructionPP == null
                                                            ? 0
                                                            : pdbInfoProvider(instructionPP)));
                });
            return this;
        }
        public InterceptorParameterConfigurator<T> AndInjectThePdbInfo(Func<SequencePoint, string> pdbInfoProvider)
        {
            _myGenerator.Generators.Add((parameter, instructions, info) =>
                {
                    SequencePoint instructionPP = _interceptor.Instruction.GetLastSequencePoint();
                    instructions.Add(Instruction.Create(OpCodes.Ldstr,
                                                        instructionPP == null
                                                            ? null
                                                            : pdbInfoProvider(instructionPP)));
                });
            return this;
        }

        
    }
}