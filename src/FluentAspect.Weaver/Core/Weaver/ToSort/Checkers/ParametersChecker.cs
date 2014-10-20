using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.Checkers
{
   public static class ParametersChecker
   {
      public static void Check<T>(this InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP, IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
           where T : VariablesForMethod
      {
         CheckDuplicates(errorHandler, interceptorParameterPossibilitiesP);
         foreach (var parameterInfo in parameters)
         {
            CheckParameter(errorHandler, interceptorParameterPossibilitiesP, parameterInfo);
         }
      }

      private static void CheckParameter<T>(ErrorHandler errorHandler, InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP, ParameterInfo parameterInfo) where T : VariablesForMethod
      {
         string key_L = parameterInfo.Name.ToLower();
         try
         {
            interceptorParameterPossibilitiesP.PossibleParameters.First(p => p.Name == key_L).Check(parameterInfo, errorHandler);
         }
         catch (Exception)
         {
            string expectedParameterNames = String.Join(", ", (from p in interceptorParameterPossibilitiesP.PossibleParameters select p.Name).ToArray());
            errorHandler.OnError(ErrorCode.UnknownParameter, FileLocation.None, parameterInfo.Name, expectedParameterNames);
         }
      }

      private static void CheckDuplicates<T>(ErrorHandler errorHandler, InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP) where T : VariablesForMethod
      {
          IEnumerable<InterceptorParameterPossibility<T>> duplicates =
            interceptorParameterPossibilitiesP.PossibleParameters.GroupBy(s => s.Name).SelectMany(grp => grp.Skip(1));
          foreach (InterceptorParameterPossibility<T> duplicate in duplicates)
         {
            errorHandler.OnError(ErrorCode.ParameterAlreadyDeclared, FileLocation.None, duplicate.Name);
         }
      }
   }
}
