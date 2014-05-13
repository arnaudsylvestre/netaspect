using System;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Helpers.IL;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
    internal static class Ensure
    {
        public static void NotReferenced(ParameterInfo parameterInfo, IErrorListener errorHandler)
        {
            if (parameterInfo.ParameterType.IsByRef)
            {
                errorHandler.OnError(ErrorCode.ImpossibleToReferenceTheParameter, FileLocation.None, parameterInfo.Name, parameterInfo.Member.Name,
                                     parameterInfo.Member.DeclaringType.FullName);
            }
        }

        public static void SequencePoint(Instruction instruction, ErrorHandler errorHandler, ParameterInfo info)
        {
            if (instruction.GetLastSequencePoint() == null)
                errorHandler.OnError(ErrorCode.NoDebuggingInformationAvailable, FileLocation.None,
                        info.Name, (info.Member).Name, (info.Member.DeclaringType).FullName);
        }

        //public static void ParameterType<T>(ParameterInfo info, ErrorHandler handler)
        //{
        //    if (info.ParameterType != typeof (T))
        //        handler.Errors.Add(string.Format("Wrong parameter type for {0} in method {1} of type {2}", info.Name,
        //                                         info.Member.Name, info.Member.DeclaringType.FullName));
        //}

        //public static void ParameterType(ParameterInfo info, ErrorHandler errorHandler, TypeReference declaringType,
        //                                 Type type)
        //{
        //    bool secondTypeOk = true;
        //    if (type != null)
        //        secondTypeOk = info.ParameterType == type;
        //    if (info.ParameterType.FullName != declaringType.FullName && !secondTypeOk)
        //        errorHandler.Errors.Add(string.Format("Wrong parameter type for {0} in method {1} of type {2}",
        //                                              info.Name, info.Member.Name, info.Member.DeclaringType.FullName));
        //}

        


        //public static void OfType<T>(ParameterInfo info, ErrorHandler handler)
        //{
        //    OfType(info, handler, typeof (T).FullName);
        //}


        public static void NotOut(ParameterInfo parameterInfo, ErrorHandler errorHandler)
        {
            if (parameterInfo.IsOut)
            {
                errorHandler.OnError(ErrorCode.ImpossibleToOutTheParameter, FileLocation.None,
                                     parameterInfo.Name, parameterInfo.Member.Name,
                                     parameterInfo.Member.DeclaringType.FullName);
            }
        }

        public static void ResultOfType(ParameterInfo info, ErrorHandler handler, MethodDefinition method)
        {
            if (MethodMustNotBeVoid(info, handler, method)) return;
            NotOut(info, handler);
            if (info.ParameterType == typeof (object))
                return;
            if (info.ParameterType.FullName.Replace("&", "") != method.ReturnType.FullName.Replace("/", "+"))
            {
                handler.OnError(
                    ErrorCode.ParameterWithBadTypeBecauseReturnMethod, FileLocation.None,
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName.Replace("/", "+"),
                    info.ParameterType.FullName,
                    method.ReturnType.FullName, method.Name, method.DeclaringType.FullName.Replace("/", "+"));
            }
        }

        private static bool MethodMustNotBeVoid(ParameterInfo info, ErrorHandler handler, MethodDefinition method)
        {
            if (method.ReturnType == method.Module.TypeSystem.Void)
            {
                handler.OnError(
                    ErrorCode.MustNotBeVoid, FileLocation.None,
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName.Replace("/", "+"), method.Name,
                    method.DeclaringType.FullName.Replace("/", "+"));
                return true;
            }
            return false;
        }

        //public static void OfType(ParameterInfo info, IErrorListener handler, params string[] types)
        //{
        //    if (
        //        !(from t in types where info.ParameterType.FullName.Replace("&", "") == t.Replace("/", "+") select t)
        //             .Any())
        //    {
        //        handler.OnError(
        //            "the {0} parameter in the method {1} of the type '{2}' is declared with the type '{3}' but it is expected to be {4}",
        //            info.Name, info.Member.Name, info.Member.DeclaringType.FullName, info.ParameterType.FullName,
        //            string.Join(" or ", types.Select(type => type.Replace("/", "+")).ToArray()));
        //    }
        //}

        public static void OfType(ParameterInfo info, ErrorHandler handler, ParameterDefinition parameter)
        {
            if (parameter.ParameterType.IsGenericParameter && info.ParameterType.IsByRef)
            {
                handler.OnError(ErrorCode.ImpossibleToRefGenericParameter, FileLocation.None);
                return;
            }


            if (info.ParameterType == typeof (object))
                return;
            if (info.ParameterType.FullName.Replace("&", "") !=
                parameter.ParameterType.FullName.Replace("&", "").Replace("/", "+"))
            {
                handler.OnError(
                    ErrorCode.ParameterWithBadType, FileLocation.None,
                    info.Name, info.Member.Name, info.Member.DeclaringType.FullName.Replace("/", "+"),
                    info.ParameterType.FullName,
                    parameter.ParameterType.FullName.Replace("/", "+"), ((IMemberDefinition) parameter.Method).Name,
                    ((IMemberDefinition) parameter.Method).DeclaringType.FullName.Replace("/", "+"));
            }
        }

        public static void NotStatic(ParameterInfo parameter, IErrorListener handler, MethodDefinition definition)
        {
            if (definition.IsStatic)
                handler.OnError(ErrorCode.ParameterCanNotBeUsedInStaticMethod, FileLocation.None, parameter.Name);
        }

        public static void NotStatic(ParameterInfo parameter, IErrorListener handler, FieldDefinition definition)
        {
            if (definition.IsStatic)
               handler.OnError(ErrorCode.ParameterCanNotBeUsedInStaticMethod, FileLocation.None, parameter.Name);
        }

        public static void NotStaticButDefaultValue(ParameterInfo parameter, IErrorListener handler, FieldDefinition definition)
        {
            if (definition.IsStatic)
            {
                if (definition.DeclaringType.IsValueType)
                    handler.OnError(ErrorCode.NotAvailableInStaticStruct, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
                else
                    handler.OnError(ErrorCode.NotAvailableInStatic, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
            }

        }

        public static void NotStaticButDefaultValue(ParameterInfo parameter, IErrorListener handler, IMemberDefinition member)
        {
            if ((bool) member.GetType().GetProperty("IsStatic").GetValue(member, new object[0]))
            {
                if (member.DeclaringType.IsValueType)
                    handler.OnError(ErrorCode.NotAvailableInStaticStruct, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
                else
                    handler.OnError(ErrorCode.NotAvailableInStatic, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
            }

        }

        public static void NotStaticButDefaultValue(ParameterInfo parameter, IErrorListener handler, PropertyDefinition definition)
        {
            if (definition.GetMethod.IsStatic)
            {
                if (definition.DeclaringType.IsValueType)
                    handler.OnError(ErrorCode.NotAvailableInStaticStruct, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
                else
                    handler.OnError(ErrorCode.NotAvailableInStatic, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
            }

        }
        public static void NotStaticButDefaultValue(ParameterInfo parameter, IErrorListener handler, MethodDefinition definition)
        {
            if (definition.IsStatic)
            {
                if (definition.DeclaringType.IsValueType)
                    handler.OnError(ErrorCode.NotAvailableInStaticStruct, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
                else
                   handler.OnError(ErrorCode.NotAvailableInStatic, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
            }

        }
    }
}