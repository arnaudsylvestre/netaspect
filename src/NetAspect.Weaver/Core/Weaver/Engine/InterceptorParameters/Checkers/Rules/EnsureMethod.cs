using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Checkers
{
    internal static class EnsureMethod
   {
      public static void IsNotStatic(ParameterInfo parameter, ErrorHandler handler, MethodDefinition definition)
      {
         if (definition.IsStatic)
             handler.OnError(ErrorCode.ParameterCanNotBeUsedInStaticMethod, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName, definition.Name, definition.DeclaringType.FullName.Replace('/', '+'));
      }

       public static void IsNotStaticButDefaultValue(ParameterInfo parameter, ErrorHandler handler, IMemberDefinition member)
      {
         if ((bool) member.GetType().GetProperty("IsStatic").GetValue(member, new object[0]))
         {
            if (member.DeclaringType.IsValueType)
               handler.OnError(ErrorCode.NotAvailableInStaticStruct, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
            else
               handler.OnError(ErrorCode.NotAvailableInStatic, FileLocation.None, parameter.Name, parameter.Member.Name, parameter.Member.DeclaringType.FullName);
         }
      }

   }
}
