using System;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Helpers.IL;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Checkers
{
    static class Ensure
    {
        public static void SequencePoint(Instruction instruction, ErrorHandler errorHandler, ParameterInfo info)
        {
            if (instruction.GetLastSequencePoint() == null)
                errorHandler.Warnings.Add(string.Format("The parameter {0} in method {1} of type {2} will have the default value because there is no debugging information",
                    info.Name, (info.Member).Name, (info.Member.DeclaringType).FullName));
        }

        public static void ParameterType<T>(ParameterInfo info, ErrorHandler handler)
        {
            if (info.ParameterType != typeof(T))
                handler.Errors.Add(string.Format("Wrong parameter type for {0} in method {1} of type {2}", info.Name, info.Member.Name, info.Member.DeclaringType.FullName));
        }

        public static void ParameterType(ParameterInfo info, ErrorHandler errorHandler, TypeReference declaringType, Type type)
        {
            var secondTypeOk = true;
            if (type != null)
                secondTypeOk = info.ParameterType == type;
            if (info.ParameterType.FullName != declaringType.FullName && !secondTypeOk)
                errorHandler.Errors.Add(string.Format("Wrong parameter type for {0} in method {1} of type {2}", info.Name, info.Member.Name, info.Member.DeclaringType.FullName));
        }
    }
}
