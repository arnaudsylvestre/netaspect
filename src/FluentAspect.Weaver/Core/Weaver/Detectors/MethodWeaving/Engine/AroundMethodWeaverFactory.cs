using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving
{
    public interface IInterceptorAroundMethodBuilder
    {
        void FillCommon(AroundMethodInfo info);
        void FillBeforeSpecific(AroundMethodInfo info);
        void FillAfterSpecific(AroundMethodInfo info);
        void FillOnExceptionSpecific(AroundMethodInfo info);
        void FillOnFinallySpecific(AroundMethodInfo info);
    }

    public class AroundMethodWeaverFactory : IAroundMethodWeaverFactory
    {
        private IInterceptorAroundMethodBuilder builder;

        public AroundMethodWeaverFactory(IInterceptorAroundMethodBuilder builder)
        {
            this.builder = builder;
        }

        public IIlInjector<IlInjectorAvailableVariables> Create(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect,
            Action<AroundMethodInfo> fillSpecific)
        {
            var checker = new ParametersChecker();
            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();


            AroundMethodInfo info = new AroundMethodInfo()
                {
                    Generator = parametersIlGenerator,
                    Interceptor                    = interceptorMethod,
                    Method                    = method,
                };
            builder.FillCommon(info);
            fillSpecific(info);

            return new MethodWeavingBeforeMethodInjector<IlInjectorAvailableVariables>(method, interceptorMethod, checker,
                                                                                       parametersIlGenerator, aspect);
        }

        public IIlInjector<IlInjectorAvailableVariables> CreateForBefore(MethodDefinition method,
                                                                                MethodInfo interceptorMethod,
                                                                                NetAspectDefinition aspect)
        {
            return Create(method, interceptorMethod, aspect, info => builder.FillBeforeSpecific(info));
        }

        public IIlInjector<IlInjectorAvailableVariables> CreateForOnFinally(MethodDefinition method,
                                                                                   MethodInfo interceptorMethod,
                                                                                   NetAspectDefinition aspect)
        {
            return Create(method, interceptorMethod, aspect, info => builder.FillOnFinallySpecific(info));
        }

        private static void FillCommon(MethodDefinition method,
                                       ParametersIlGenerator<IlInjectorAvailableVariables> parametersIlGenerator)
        {
            parametersIlGenerator.CreateIlGeneratorForInstanceParameter(method);
            parametersIlGenerator.CreateIlGeneratorForMethodParameter();
            parametersIlGenerator.CreateIlGeneratorForParametersParameter(method);
            parametersIlGenerator.CreateIlGeneratorForParameterNameParameter(method);
        }

        private static void FillCommon(MethodDefinition method, ParametersChecker checker)
        {
            
        }

        public IIlInjector<IlInjectorAvailableVariables> CreateForAfter(MethodDefinition method,
                                                                               MethodInfo interceptorMethod,
                                                                               NetAspectDefinition aspect)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);
            checker.CreateCheckerForResultParameter(method);


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);
            return Create(method, interceptorMethod, aspect, info => builder.FillAfterSpecific(info));
        }

        public IIlInjector<IlInjectorAvailableVariables> CreateForExceptions(MethodDefinition method,
                                                                                     MethodInfo interceptorMethod,
                                                                                     NetAspectDefinition aspect)
        {
            var checker = new ParametersChecker();
            FillCommon(method, checker);
            checker.CreateCheckerForExceptionParameter();


            var parametersIlGenerator = new ParametersIlGenerator<IlInjectorAvailableVariables>();
            FillCommon(method, parametersIlGenerator);
            return Create(method, interceptorMethod, aspect, info => builder.FillOnExceptionSpecific(info));
        }
    }
}