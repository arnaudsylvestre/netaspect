using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAspect.Weaver.Core.Errors;

namespace FluentAspect.Weaver.Core.Weavers.CallWeaving.Engine
{
    public class ParametersEngine
    {

        private class 

        Dictionary<string, Action<ParameterInfo, ErrorHandler>> possibleParameters = new Dictionary<string, Action<ParameterInfo, ErrorHandler>>();

        public void AddPossibleParameter(string linenumber, Action<ParameterInfo, ErrorHandler> check)
        {
            possibleParameters.Add(linenumber, check);
        }

        public void Check(IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
        {
            foreach (var parameterInfo in parameters)
            {
                possibleParameters[parameterInfo.Name.ToLower()](parameterInfo, errorHandler);
            }
        }

        public void Fill(IEnumerable<ParameterInfo> parameters, ErrorHandler errorHandler)
        {
            foreach (ParameterInfo parameterInfo_L in parameters)
            {
                possibleParameters[parameterInfo_L.Name.ToLower()](parameterInfo_L, errorHandler);
            }
        }
    }
}