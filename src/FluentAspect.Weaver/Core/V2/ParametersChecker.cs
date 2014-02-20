using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;
using FluentAspect.Weaver.Core.Weavers.MethodWeaving.Factory.Parameters;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class ParametersChecker
    {

        readonly Dictionary<string, IInterceptorParameterChecker> possibleParameters = new Dictionary<string, IInterceptorParameterChecker>();

        public void AddPossibleParameter(string linenumber, IInterceptorParameterChecker checker)
        {
            possibleParameters.Add(linenumber, checker);
        }

        public void Check(IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
        {
            foreach (var parameterInfo in parameters)
            {
               var key_L = parameterInfo.Name.ToLower();
               if (possibleParameters.ContainsKey(key_L))
               {
                  possibleParameters[key_L].Check(parameterInfo, errorHandler);
               }
               else
               {
                  errorHandler.Errors.Add(string.Format("The parameter '{0}' is unknown", parameterInfo.Name));
               }
            }
        }
    }
}