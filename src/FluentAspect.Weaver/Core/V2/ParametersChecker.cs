using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class ParametersChecker
    {
        private List<InterceptorParametersChecker> possibleParameters = new List<InterceptorParametersChecker>();

        public void Add(InterceptorParametersChecker checker)
        {
            possibleParameters.Add(checker);
        }

        public void Check(IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
        {

            var duplicates = possibleParameters.GroupBy(s => s.ParameterName).SelectMany(grp => grp.Skip(1));
            foreach (var duplicate in duplicates)
            {
                errorHandler.Errors.Add(string.Format("The parameter {0} is already declared", duplicate.ParameterName));
            }
            foreach (var parameterInfo in parameters)
            {
               var key_L = parameterInfo.Name.ToLower();
                try
                {
                    possibleParameters.Find(p => p.ParameterName == key_L).Checker.Check(parameterInfo, errorHandler);
                }
                catch (Exception)
                {
                  errorHandler.Errors.Add(string.Format("The parameter '{0}' is unknown", parameterInfo.Name));
               }
            }
        }

        public void AddRange(IEnumerable<InterceptorParametersChecker> s)
        {
            foreach (var interceptorParametersChecker in s)
            {
                Add(interceptorParametersChecker);
            }
        }
    }
}