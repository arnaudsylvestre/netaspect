using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Checkers
{
    public static class EnsureParameter
    {
        public static void IsNotReferenced(ParameterInfo parameterInfo, ErrorHandler errorHandler)
        {
            if (!parameterInfo.ParameterType.IsByRef) return;
            errorHandler.OnError(
                ErrorCode.ImpossibleToReferenceTheParameter,
                FileLocation.None,
                parameterInfo.Name,
                parameterInfo.Member.Name,
                parameterInfo.Member.DeclaringType.FullName);
        }


        public static void IsNotOut(ParameterInfo parameterInfo, ErrorHandler errorHandler)
        {
            if (!parameterInfo.IsOut) return;
            errorHandler.OnError(
                ErrorCode.ImpossibleToOutTheParameter,
                FileLocation.None,
                parameterInfo.Name,
                parameterInfo.Member.Name,
                parameterInfo.Member.DeclaringType.FullName);
        }

        public static void IsOfType(ParameterInfo info, ErrorHandler handler, ParameterDefinition parameter)
        {
            if (parameter.ParameterType.IsGenericParameter && info.ParameterType.IsByRef)
            {
                handler.OnError(ErrorCode.ImpossibleToRefGenericParameter, FileLocation.None, info.Name, info.Member.Name, info.Member.DeclaringType.FullName, ((MethodReference)parameter.Method).Name, ((MethodReference)parameter.Method).DeclaringType.FullName.Replace('/', '+'));
                return;
            }


            if (info.ParameterType == typeof(object))
                return;
            if (info.ParameterType.FullName.Replace("&", "") !=
                parameter.ParameterType.FullName.Replace("&", "").Replace("/", "+"))
            {
                handler.OnError(
                   ErrorCode.ParameterWithBadType,
                   FileLocation.None,
                   info.Name,
                   info.Member.Name,
                   info.Member.DeclaringType.FullName.Replace("/", "+"),
                   info.ParameterType.FullName,
                   parameter.ParameterType.FullName.Replace("/", "+"),
                   ((IMemberDefinition)parameter.Method).Name,
                   ((IMemberDefinition)parameter.Method).DeclaringType.FullName.Replace("/", "+"));
            }
        }
    }
}