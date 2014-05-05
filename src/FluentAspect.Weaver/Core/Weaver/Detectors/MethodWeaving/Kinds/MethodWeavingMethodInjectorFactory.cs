using NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Engine;

namespace NetAspect.Weaver.Core.Weaver.Detectors.MethodWeaving.Method
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