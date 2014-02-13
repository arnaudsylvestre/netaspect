using System;
using System.Linq;
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

        public static void NotReferenced(ParameterInfo parameterInfo, ErrorHandler errorHandler)
        {
            if (parameterInfo.ParameterType.IsByRef)
            {
                errorHandler.Errors.Add(string.Format("impossible to ref/out the parameter '{0}' in the method {1} of the type '{2}'", parameterInfo.Name, parameterInfo.Member.Name, parameterInfo.Member.DeclaringType.FullName));
            }
        }



        public static void OfType<T>(ParameterInfo info, ErrorHandler handler)
        {
            Ensure.OfType(info, handler, typeof(T).FullName);
        }



        public static void ResultOfType(ParameterInfo info, ErrorHandler handler, MethodDefinition method)
        {
           if (info.ParameterType.FullName.Replace("&", "") != method.ReturnType.FullName.Replace("/", "+"))
            {
                handler.Errors.Add(string.Format("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4} because the return type of the method {5} in the type {6}",
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName, info.ParameterType.FullName,
                    method.ReturnType.FullName, method.Name, method.DeclaringType.FullName));
            }
        }

        public static void OfType(ParameterInfo info, ErrorHandler handler, params string[] types)
        {
           if (!(from t in types where info.ParameterType.FullName.Replace("&", "") == t.Replace("/", "+") select t).Any())
            {
                handler.Errors.Add(string.Format("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4}",
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName, info.ParameterType.FullName,
                    string.Join(" or ", types.Select(type => type.Replace("/", "+")).ToArray())));
            }
        }

        public static void OfType(ParameterInfo info, ErrorHandler handler, ParameterDefinition parameter)
        {
            if (parameter.ParameterType.IsGenericParameter && info.ParameterType.IsByRef)
            {
                handler.Errors.Add(string.Format("Impossible to ref a generic parameter"));
                return;
            }


            if (info.ParameterType == typeof(object))
                return;
            if (info.ParameterType.FullName.Replace("&", "") != parameter.ParameterType.FullName.Replace("&", "").Replace("/", "+"))
            {
                handler.Errors.Add(string.Format("the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4} because of the type of this parameter in the method {5} of the type {6}",
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName, info.ParameterType.FullName,
                    parameter.ParameterType.FullName, ((IMemberDefinition)parameter.Method).Name, ((IMemberDefinition)parameter.Method).DeclaringType.FullName));
            }
        }
    }
}
