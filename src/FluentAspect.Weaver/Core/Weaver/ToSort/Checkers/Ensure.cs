using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Helpers.Mono.Cecil.IL;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Checkers
{
   internal static class Ensure
   {
      public static void NotReferenced(ParameterInfo parameterInfo, ErrorHandler errorHandler)
      {
         if (parameterInfo.ParameterType.IsByRef)
         {
            errorHandler.OnError(
               ErrorCode.ImpossibleToReferenceTheParameter,
               FileLocation.None,
               parameterInfo.Name,
               parameterInfo.Member.Name,
               parameterInfo.Member.DeclaringType.FullName);
         }
      }

      public static void SequencePoint(Mono.Cecil.Cil.Instruction instruction, ErrorHandler errorHandler, ParameterInfo info)
      {
         if (instruction.GetLastSequencePoint() == null)
            errorHandler.OnError(
               ErrorCode.NoDebuggingInformationAvailable,
               FileLocation.None,
               info.Name,
               (info.Member).Name,
               (info.Member.DeclaringType).FullName);
      }
       
      public static void NotOut(ParameterInfo parameterInfo, ErrorHandler errorHandler)
      {
         if (parameterInfo.IsOut)
         {
            errorHandler.OnError(
               ErrorCode.ImpossibleToOutTheParameter,
               FileLocation.None,
               parameterInfo.Name,
               parameterInfo.Member.Name,
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
               ErrorCode.ParameterWithBadTypeBecauseReturnMethod,
               FileLocation.None,
               info.Name,
               info.Member.Name,
               info.Member.DeclaringType.FullName.Replace("/", "+"),
               info.ParameterType.FullName,
               method.ReturnType.FullName,
               method.Name,
               method.DeclaringType.FullName.Replace("/", "+"));
         }
      }

      private static bool MethodMustNotBeVoid(ParameterInfo info, ErrorHandler handler, MethodDefinition method)
      {
         if (method.ReturnType == method.Module.TypeSystem.Void)
         {
            handler.OnError(
               ErrorCode.MustNotBeVoid,
               FileLocation.None,
               info.Name,
               info.Member.Name,
               info.Member.DeclaringType.FullName.Replace("/", "+"),
               method.Name,
               method.DeclaringType.FullName.Replace("/", "+"));
            return true;
         }
         return false;
      }

      public static void ParameterOfType(ParameterInfo info, ErrorHandler handler, ParameterDefinition parameter)
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
               ErrorCode.ParameterWithBadType,
               FileLocation.None,
               info.Name,
               info.Member.Name,
               info.Member.DeclaringType.FullName.Replace("/", "+"),
               info.ParameterType.FullName,
               parameter.ParameterType.FullName.Replace("/", "+"),
               ((IMemberDefinition) parameter.Method).Name,
               ((IMemberDefinition) parameter.Method).DeclaringType.FullName.Replace("/", "+"));
         }
      }

      public static void NotStatic(ParameterInfo parameter, ErrorHandler handler, MethodDefinition definition)
      {
         if (definition.IsStatic)
            handler.OnError(ErrorCode.ParameterCanNotBeUsedInStaticMethod, FileLocation.None, parameter.Name);
      }

       public static void NotStaticButDefaultValue(ParameterInfo parameter, ErrorHandler handler, IMemberDefinition member)
      {
         if ((bool) member.GetType().GetProperty("IsStatic").GetValue(member, new object[0]))
         {
            if (member.DeclaringType.IsValueType)
               handler.OnError(ErrorCode.NotAvailableInStaticStruct, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
            else
               handler.OnError(ErrorCode.NotAvailableInStatic, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
         }
      }

       public static void ResultOfTypeNotReferenced(ParameterInfo parameter_P, ErrorHandler errorListener_P, MethodDefinition method_P)
      {
         ResultOfType(parameter_P, errorListener_P, method_P);
         NotReferenced(parameter_P, errorListener_P);
      }
   }
}
