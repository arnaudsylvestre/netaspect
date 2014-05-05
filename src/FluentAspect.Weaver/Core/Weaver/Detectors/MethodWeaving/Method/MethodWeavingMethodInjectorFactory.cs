using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Model.Aspect;
using NetAspect.Weaver.Core.Model.Weaving;
using NetAspect.Weaver.Core.Weaver.Checkers;
using NetAspect.Weaver.Core.Weaver.Detectors.CallWeaving.Engine;
using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving;
using NetAspect.Weaver.Core.Weaver.Engine;
using NetAspect.Weaver.Core.Weaver.Generators;

namespace NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method
{
    public class MethodWeavingMethodInjectorFactory : IInterceptorAroundMethodBuilder
    {
        public void FillCommon(AroundMethodInfo info)
        {
            info.AddInstance()
                .AddCurrentMethod()


            checker.CreateCheckerForMethodParameter();
            checker.CreateCheckerForParameterNameParameter(method);
            checker.CreateCheckerForParametersParameter();



            //parametersIlGenerator.CreateIlGeneratorForResultParameter();

            //parametersIlGenerator.CreateIlGeneratorForExceptionParameter();
        }

        public void FillBeforeSpecific(AroundMethodInfo info)
        {

        }

        public void FillAfterSpecific(AroundMethodInfo info)
        {

        }

        public void FillOnExceptionSpecific(AroundMethodInfo info)
        {

        }

        public void FillOnFinallySpecific(AroundMethodInfo info)
        {

        }
    }
}