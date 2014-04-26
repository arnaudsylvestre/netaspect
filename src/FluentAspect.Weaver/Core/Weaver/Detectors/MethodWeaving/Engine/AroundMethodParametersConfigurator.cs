using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine
{
    public class AroundMethodParametersConfigurator<T>
    {
        private readonly AroundInstructionInfoExtensions.MyGenerator<T> _myGenerator;
        private readonly AroundInstructionInfoExtensions.MyInterceptorParameterChecker _checker;
        private readonly AroundMethodInfo _aroundInstruction;

        private List<string> allowedTypes = new List<string>();

        public AroundInstructionInfoExtensions.MyGenerator<T> Generator
        {
            get { return _myGenerator; }
        }

        public AroundMethodInfo AroundInstruction
        {
            get { return _aroundInstruction; }
        }

        public AroundMethodParametersConfigurator(AroundInstructionInfoExtensions.MyGenerator<T> myGenerator, AroundInstructionInfoExtensions.MyInterceptorParameterChecker checker, AroundMethodInfo aroundInstruction)
        {
            _myGenerator = myGenerator;
            _checker = checker;
            _aroundInstruction = aroundInstruction;
            _checker.Checkers.Add((info, handler) => Ensure.OfType(info, handler, allowedTypes.ToArray()));
        }
    }
}