using System;
using System.Reflection;
using Mono.Cecil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
   public class ParameterTypeChecker : IChecker
   {
      private readonly ParameterDefinition parameterDefinition;

      public ParameterTypeChecker(string expectedType, ParameterDefinition parameterDefinition)
      {
         ExpectedType = expectedType;
         this.parameterDefinition = parameterDefinition;
      }

      private string ExpectedType { get; set; }

      public void Check(ParameterInfo parameter, ErrorHandler errorHandler)
      {
         CheckGenericType(parameter, errorHandler);
         CheckType(parameter, errorHandler);
      }

      public void CheckType(ParameterInfo parameterInfo, ErrorHandler errorHandler)
      {
         Type parameterType = parameterInfo.ParameterType;
         if (parameterType.FullName.Replace("&", "") == typeof (object).FullName)
            return;
         if (parameterType.FullName.Replace("&", "") != Clean(ExpectedType))
         {
            errorHandler.OnError(
               ErrorCode.ParameterWithBadType,
               FileLocation.None,
               parameterInfo.Name,
               parameterInfo.Member.Name,
               parameterInfo.Member.DeclaringType.FullName.Replace("/", "+"),
               parameterInfo.ParameterType.FullName,
               ExpectedType.Replace("/", "+"));
         }
      }

      private static string Clean(string expectedType)
      {
         return expectedType.Replace("/", "+").Replace("&", "");
      }

      public void CheckGenericType(ParameterInfo info, ErrorHandler errorHandler)
      {
         if (parameterDefinition == null)
            return;
         if (parameterDefinition.ParameterType.IsGenericParameter && info.ParameterType.IsByRef)
         {
            errorHandler.OnError(ErrorCode.ImpossibleToRefGenericParameter, FileLocation.None);
         }
      }
   }
}
