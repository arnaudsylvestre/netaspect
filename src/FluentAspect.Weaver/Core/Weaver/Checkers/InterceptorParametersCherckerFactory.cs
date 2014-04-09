using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Called;
using NetAspect.Weaver.Core.Weaver.Checkers.CallWeaving.Source;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Exception;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Instance;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Member;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Parameters;
using NetAspect.Weaver.Core.Weaver.Checkers.MethodWeaving.Result;
using NetAspect.Weaver.Core.Weaver.Engine;

namespace NetAspect.Weaver.Core.Weaver.Checkers
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



        //private static void AddLineNumber(this ParametersEngine engine, JoinPoint point)
        //{
        //    engine.AddPossibleParameter("linenumber", (info, handler) =>
        //    {
        //        Ensure.SequencePoint(point.InstructionStart, handler, info);
        //        Ensure.ParameterType<int>(info, handler);
        //    },
        //                                (info, instructions) =>
        //                                instructions.Add(
        //                                    InstructionFactory.Create(point.InstructionStart.GetLastSequencePoint(),
        //                                                              i => i.StartLine)));
        //}



        //private static void AddFilename(this ParametersEngine engine, JoinPoint point)
        //{
        //    engine.AddPossibleParameter("filename", (info, handler) =>
        //    {
        //        Ensure.SequencePoint(point.InstructionStart, handler, info);
        //        Ensure.ParameterType<string>(info, handler);
        //    },
        //                                (info, instructions) =>
        //                                instructions.Add(
        //                                    InstructionFactory.Create(point.InstructionStart.GetLastSequencePoint(),
        //                                                              i => Path.GetFileName(i.Document.Url))));
        //}


        //private static void AddFilepath(this ParametersEngine engine, JoinPoint joinPoint)
        //{
        //    engine.AddPossibleParameter("filepath", (info, handler) =>
        //    {
        //        Ensure.SequencePoint(joinPoint.InstructionStart, handler, info);
        //        Ensure.ParameterType<string>(info, handler);
        //    },
        //                                (info, instructions) =>
        //                                instructions.Add(
        //                                    InstructionFactory.Create(
        //                                        joinPoint.InstructionStart.GetLastSequencePoint(), i => i.Document.Url)));
        //}



        public static void CreateCheckerForColumnNumberParameter(this ParametersChecker checkers, Instruction instruction)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "columnnumber",
                Checker = new ColumnNumberInterceptorParametersChercker(instruction),
            });
        }

        public static void CreateCheckerForLineNumberParameter(this ParametersChecker checkers, Instruction instruction)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "linenumber",
                Checker = new ColumnNumberInterceptorParametersChercker(instruction),
            });
        }
        public static void CreateCheckerForFilenameParameter(this ParametersChecker checkers, Instruction instruction)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "filename",
                Checker = new FilenameInterceptorParametersChercker(instruction),
            });
        }
        public static void CreateCheckerForFilePathParameter(this ParametersChecker checkers, Instruction instruction)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "filepath",
                Checker = new FilenameInterceptorParametersChercker(instruction),
            });
        }

        public static void CreateCheckerForField(this ParametersChecker checkers)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "field",
                Checker = new FieldInterceptorParametersChercker(),
            });
        }
        public static void CreateCheckerForCallerParameters(this ParametersChecker checkers, MethodDefinition method)
        {

            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "callerparameters",
                Checker = new ParametersInterceptorParametersChercker(),
            });
        }
        public static void CreateCheckerForCalledParameters(this ParametersChecker checkers, MethodDefinition method)
        {

            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "calledparameters",
                Checker = new ParametersInterceptorParametersChercker(),
            });
        }
        public static void CreateCheckerForCalledParametersName(this ParametersChecker checkers, MethodDefinition method)
        {
            checkers.AddRange(method.Parameters.Select(parameter => new InterceptorParametersChecker
            {
                ParameterName = "called" + parameter.Name.ToLower(),
                Checker = new CalledParameterNameInterceptorParametersChercker(parameter),
            }));
        }
        public static void CreateCheckerForCallerParametersName(this ParametersChecker checkers, MethodDefinition callerMethod)
        {
            checkers.AddRange(callerMethod.Parameters.Select(parameter => new InterceptorParametersChecker
            {
                ParameterName = "caller" + parameter.Name.ToLower(),
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
        public static void CreateCheckerForCalledParameter(this ParametersChecker checkers, MethodDefinition calledType)
        {
            checkers.Add(new InterceptorParametersChecker
            {
                ParameterName = "called",
                Checker = new CalledInterceptorForMethodParametersChercker(calledType),
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