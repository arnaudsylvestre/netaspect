using FluentAspect.Weaver.Core.V2.Weaver.Call;
using FluentAspect.Weaver.Core.V2.Weaver.Method;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Generators
{
    public static class InterceptorParametersIlGeneratorFactory
    {
        public static void CreateIlGeneratorForInstanceParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                    MethodDefinition method)
        {
            ilGeneratoir.Add("instance", new InstanceInterceptorParametersIlGenerator<T>());
        }
        public static void CreateIlGeneratorForCallerParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                    MethodDefinition method)
        {
            ilGeneratoir.Add("caller", new InstanceInterceptorParametersIlGenerator<T>());
        }
        public static void CreateIlGeneratorForCalledParametersName(this ParametersIlGenerator<IlInstructionInjectorAvailableVariables> ilGeneratoir,
                                                                    MethodDefinition calledMethod)
        {
            ilGeneratoir.

            foreach (ParameterDefinition parameterDefinition in calledMethod.Parameters)
            {
                ilGeneratoir.Add(parameterDefinition.Name.ToLower(),
                                 new ParameterNameInterceptorParametersIlGenerator<IlInstructionInjectorAvailableVariables>(parameterDefinition));
            }
        }

        public static void CreateIlGeneratorForMethodParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("method", new MethodInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForPropertyParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("property", new PropertyInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForResultParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("result", new ResultInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForExceptionParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir)
        {
            ilGeneratoir.Add("exception", new ExceptionInterceptorParametersIlGenerator());
        }

        public static void CreateIlGeneratorForParameterNameParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                         MethodDefinition method)
        {
            foreach (ParameterDefinition parameterDefinition in method.Parameters)
            {
                ilGeneratoir.Add(parameterDefinition.Name.ToLower(),
                                 new ParameterNameInterceptorParametersIlGenerator<T>(parameterDefinition));
            }
        }


        public static void CreateIlGeneratorForPropertySetValueParameter<T>(this ParametersIlGenerator<T> ilGeneratoir,
                                                                            MethodDefinition method)
        {
            ilGeneratoir.Add("value", new ParameterNameInterceptorParametersIlGenerator<T>(method.Parameters[0]));
        }

        public static void CreateIlGeneratorForParametersParameter(
            this ParametersIlGenerator<IlInjectorAvailableVariables> ilGeneratoir, MethodDefinition method)
        {
            ilGeneratoir.Add("parameters", new ParametersInterceptorParametersIlGenerator());
        }
    }
}