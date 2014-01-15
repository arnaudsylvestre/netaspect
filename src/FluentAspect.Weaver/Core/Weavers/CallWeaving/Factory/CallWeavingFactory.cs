using System.Collections.Generic;
using FluentAspect.Weaver.Core.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory
{
    public static class CallWeavingFactory
    {
        public static CallMethodWeaver CreateCallMethodWeaver(MethodPoint point, IEnumerable<CallWeavingConfiguration> interceptorTypes)
         {
            var toWeave = new CallToWeave
                {
                    MethodToWeave = point.Method, Instruction = point.Instruction, Interceptors = interceptorTypes
                };
            var engine = new ParametersEngine();
            AddLineNumber(point, engine);
             AddColumnNumber(point, engine);
             AddFilename(point, engine);
             AddFilepath(engine, toWeave);
             AddCaller(engine, toWeave);

             return new CallMethodWeaver(engine, toWeave);
         }

        private static void AddCaller(ParametersEngine engine, CallToWeave toWeave)
        {
            engine.AddPossibleParameter("caller",
                                        (info, handler) =>
                                            {
                                                Ensure.ParameterType(info, handler, toWeave.MethodToWeave.DeclaringType,
                                                                     typeof (object));
                                            });
        }

        private static void AddFilepath(ParametersEngine engine, CallToWeave toWeave)
        {
            engine.AddPossibleParameter("filepath", (info, handler) =>
                {
                    Ensure.SequencePoint(toWeave.Instruction, handler, info);
                    Ensure.ParameterType<string>(info, handler);
                });
        }

        private static void AddFilename(MethodPoint point, ParametersEngine engine)
        {
            engine.AddPossibleParameter("filename", (info, handler) =>
                {
                    Ensure.SequencePoint(point.Instruction, handler, info);
                    Ensure.ParameterType<string>(info, handler);
                });
        }

        private static void AddColumnNumber(MethodPoint point, ParametersEngine engine)
        {
            engine.AddPossibleParameter("columnnumber", (info, handler) =>
                {
                    Ensure.SequencePoint(point.Instruction, handler, info);
                    Ensure.ParameterType<int>(info, handler);
                });
        }

        private static void AddLineNumber(MethodPoint point, ParametersEngine engine)
        {
            engine.AddPossibleParameter("linenumber", (info, handler) =>
                {
                    Ensure.SequencePoint(point.Instruction, handler, info);
                    Ensure.ParameterType<int>(info, handler);
                });
        }
    }
}