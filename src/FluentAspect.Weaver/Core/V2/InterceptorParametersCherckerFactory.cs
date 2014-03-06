using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using Mono.Cecil;

namespace FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters
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

    public class ParametersInterceptorParametersChercker : IInterceptorParameterChecker
    {
        public void Check(ParameterInfo parameter, IErrorListener errorListener)
        {
            Ensure.NotReferenced(parameter, errorListener);
            Ensure.OfType<object[]>(parameter, errorListener);
        }
    }
}