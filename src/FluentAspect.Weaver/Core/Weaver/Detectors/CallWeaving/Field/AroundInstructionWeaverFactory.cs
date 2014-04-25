using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Field
{
    public class AroundInstructionWeaverFactory : IAroundInstructionWeaverFactory
    {


        private IInterceptorAroundInstructionFactory _interceptorAroundInstructionFactory;
        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForBefore(MethodDefinition method, MethodInfo interceptorMethod, NetAspectDefinition aspect, Instruction instruction)
        {


            var checker = new ParametersChecker();
            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            var info = new InterceptorInfo()
                {
                    Generator = parametersIlGenerator,
                    Instruction                    = instruction,
                    Interceptor                    = interceptorMethod,
                    Method                    = method,
                };
            _interceptorAroundInstructionFactory.FillCommon(info);
            _interceptorAroundInstructionFactory.FillBeforeSpecific(info);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                                     parametersIlGenerator, aspect);
        }
        

        public IIlInjector<IlInjectorAvailableVariablesForInstruction> CreateForAfter(MethodDefinition method,
                                                                                      MethodInfo interceptorMethod,
                                                                                      NetAspectDefinition aspect, Instruction instruction)
        {
            var checker = new ParametersChecker();
            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariablesForInstruction>();
            var info = new InterceptorInfo()
                {
                    Generator = parametersIlGenerator,
                    Instruction = instruction,
                    Interceptor = interceptorMethod,
                    Method = method,
                };
            _interceptorAroundInstructionFactory.FillCommon(info);
            _interceptorAroundInstructionFactory.FillAfterSpecific(info);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariablesForInstruction>(method, interceptorMethod, checker,
                                                                                                     parametersIlGenerator, aspect);
        }
    }
}