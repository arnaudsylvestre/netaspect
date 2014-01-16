using System.IO;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Checkers;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine.Model;
using FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Helpers;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Factory.Parameters
{
    public static class PdbParameterFactory
    {
        public static void AddPdbParameters(this ParametersEngine engine, JoinPoint point)
        {
            engine.AddLineNumber(point);
            engine.AddColumnNumber(point);
            engine.AddFilename(point);
            engine.AddFilepath(point);
        }

        private static void AddLineNumber(this ParametersEngine engine, JoinPoint point)
        {

            engine.AddPossibleParameter("linenumber", (info, handler) =>
            {
                Ensure.SequencePoint(point.Instruction, handler, info);
                Ensure.ParameterType<int>(info, handler);
            },
                (info, instructions) => instructions.Add(InstructionFactory.Create(point.Instruction.GetLastSequencePoint(), i => i.StartLine)));
        }


        private static void AddColumnNumber(this ParametersEngine engine, JoinPoint point)
        {
            engine.AddPossibleParameter("columnnumber", (info, handler) =>
            {
                Ensure.SequencePoint(point.Instruction, handler, info);
                Ensure.ParameterType<int>(info, handler);
            },
                (info, instructions) => instructions.Add(InstructionFactory.Create(point.Instruction.GetLastSequencePoint(), i => i.StartColumn)));
        }


        private static void AddFilename(this ParametersEngine engine, JoinPoint point)
        {
            engine.AddPossibleParameter("filename", (info, handler) =>
            {
                Ensure.SequencePoint(point.Instruction, handler, info);
                Ensure.ParameterType<string>(info, handler);
            },
                (info, instructions) => instructions.Add(InstructionFactory.Create(point.Instruction.GetLastSequencePoint(), i => Path.GetFileName(i.Document.Url))));
        }


        private static void AddFilepath(this ParametersEngine engine, JoinPoint joinPoint)
        {
            engine.AddPossibleParameter("filepath", (info, handler) =>
            {
                Ensure.SequencePoint(joinPoint.Instruction, handler, info);
                Ensure.ParameterType<string>(info, handler);
            },
                (info, instructions) => instructions.Add(InstructionFactory.Create(joinPoint.Instruction.GetLastSequencePoint(), i => i.Document.Url)));
        }
    }
}