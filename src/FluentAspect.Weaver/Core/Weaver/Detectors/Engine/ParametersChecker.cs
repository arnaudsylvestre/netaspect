using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine
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
             errorHandler.Errors.Add(string.Format("The parameter '{0}' is unknown", parameterInfo.Name));
          }
       }

       private static void CheckDuplicates(ErrorHandler errorHandler, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
       {
          var duplicates =
             interceptorParameterConfigurations_P.PossibleParameters.GroupBy(s => s.Name).SelectMany(grp => grp.Skip(1));
          foreach (var duplicate in duplicates)
          {
             errorHandler.Errors.Add(string.Format("The parameter {0} is already declared", duplicate.Name));
          }
       }
    }
}