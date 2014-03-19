using System.Linq;
using FluentAspect.Weaver.Core.V2.Weaver.Engine;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.V2.Weaver.Checkers
{
    public static class InterceptorParametersCherckerFactory
    {
        public static void CreateCheckerForInstanceParameter(this ParametersChecker checkers, MethodDefinition method)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "instance",
                Checker = new InstanceInterceptorParametersChercker(method),
            });
        }
        public static void CreateCheckerForCallerParameter(this ParametersChecker checkers, MethodDefinition method)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "caller",
                Checker = new InstanceInterceptorParametersChercker(method),
            });
        }
        public static void CreateCheckerForCalledParametersName(this ParametersChecker checkers, MethodDefinition method)
        {
            checkers.AddRange(method.Parameters.Select(parameter => new InterceptorParametersChecker
            {
                ParameterName = parameter.Name,
                Checker = new ParameterNameInterceptorParametersChercker(parameter),
            }));
        }
        public static void CreateCheckerForCalledParameter(this ParametersChecker checkers, FieldDefinition calledType)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "called",
                Checker = new CalledInterceptorParametersChercker(calledType),
            });
        }

        public static void CreateCheckerForParametersParameter(this ParametersChecker checkers)
        {
            checkers.Add(new InterceptorParametersChecker
                {
                    ParameterName = "parameters",
                    Checker = new ParametersInterceptorParametersChercker(),
                });
        }

        public static void CreateCheckerForMethodParameter(this ParametersChecker checkers)
        {
            checkers.Add(new InterceptorParametersChecker
                {
                    ParameterName = "method",
                    Checker = new MethodInterceptorParametersChercker(),
                });
        }

        public static void CreateCheckerForPropertyParameter(this ParametersChecker checkers)
        {
            checkers.Add(new InterceptorParametersChecker
                {
                    ParameterName = "property",
                    Checker = new PropertyInterceptorParametersChercker(),
                });
        }

        public static void CreateCheckerForResultParameter(this ParametersChecker checkers, MethodDefinition method)
        {
            checkers.Add(new InterceptorParametersChecker
                {
                    ParameterName = "result",
                    Checker = new ResultInterceptorParametersChercker(method),
                });
        }

        public static void CreateCheckerForExceptionParameter(this ParametersChecker checkers)
        {
            checkers.Add(new InterceptorParametersChecker
                {
                    ParameterName = "exception",
                    Checker = new ExceptionInterceptorParametersChercker(),
                });
        }

        public static void CreateCheckerForParameterNameParameter(this ParametersChecker checkers,
                                                                  MethodDefinition methodDefinition)
        {
            checkers.AddRange(methodDefinition.Parameters.Select(parameter => new InterceptorParametersChecker
                {
                    ParameterName = parameter.Name,
                    Checker = new ParameterNameInterceptorParametersChercker(parameter),
                }));
        }

        public static void CreateCheckerForPropertySetValueParameter(this ParametersChecker checkers,
                                                                     MethodDefinition methodDefinition)
        {
            checkers.AddRange(methodDefinition.Parameters.Select(parameter => new InterceptorParametersChecker
                {
                    ParameterName = "value",
                    Checker = new ParameterNameInterceptorParametersChercker(methodDefinition.Parameters[0]),
                }));
        }
    }
}