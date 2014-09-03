using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Checkers
{
   public static class ParametersChecker
   {
      public static void Check(this InterceptorParameterConfigurations interceptorParameterConfigurations_P, IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
      {
         CheckDuplicates(errorHandler, interceptorParameterConfigurations_P);
         foreach (ParameterInfo parameterInfo in parameters)
         {
            CheckParameter(errorHandler, interceptorParameterConfigurations_P, parameterInfo);
         }
      }

      private static void CheckParameter(ErrorHandler errorHandler, InterceptorParameterConfigurations interceptorParameterConfigurations_P, ParameterInfo parameterInfo)
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

      private static void CheckDuplicates(ErrorHandler errorHandler, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
      {
         IEnumerable<InterceptorParameterConfiguration> duplicates =
            interceptorParameterConfigurations_P.PossibleParameters.GroupBy(s => s.Name).SelectMany(grp => grp.Skip(1));
         foreach (InterceptorParameterConfiguration duplicate in duplicates)
         {
            errorHandler.OnError(ErrorCode.ParameterAlreadyDeclared, FileLocation.None, duplicate.Name);
         }
      }
   }
}
