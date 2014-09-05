using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
   public static class ParametersChecker
   {
      public static void Check<T>(this InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P, IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
           where T : VariablesForMethod
      {
         CheckDuplicates(errorHandler, interceptorParameterConfigurations_P);
         foreach (ParameterInfo parameterInfo in parameters)
         {
            CheckParameter(errorHandler, interceptorParameterConfigurations_P, parameterInfo);
         }
      }

      private static void CheckParameter<T>(ErrorHandler errorHandler, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P, ParameterInfo parameterInfo) where T : VariablesForMethod
      {
         string key_L = parameterInfo.Name.ToLower();
         try
         {
            interceptorParameterConfigurations_P.PossibleParameters.First(p => p.Name == key_L).Checker.Check(parameterInfo, errorHandler);
         }
         catch (Exception)
         {
            string expectedParameterNames = String.Join(", ", (from p in interceptorParameterConfigurations_P.PossibleParameters select p.Name).ToArray());
            errorHandler.OnError(ErrorCode.UnknownParameter, FileLocation.None, parameterInfo.Name, expectedParameterNames);
         }
      }

      private static void CheckDuplicates<T>(ErrorHandler errorHandler, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P) where T : VariablesForMethod
      {
          IEnumerable<InterceptorParameterConfiguration<T>> duplicates =
            interceptorParameterConfigurations_P.PossibleParameters.GroupBy(s => s.Name).SelectMany(grp => grp.Skip(1));
          foreach (InterceptorParameterConfiguration<T> duplicate in duplicates)
         {
            errorHandler.OnError(ErrorCode.ParameterAlreadyDeclared, FileLocation.None, duplicate.Name);
         }
      }
   }
}
