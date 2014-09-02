using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Model.Errors;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine.Selectors
{
    public class SelectorParametersGenerator<T>
    {
        class PossibleParameter
        {
            public Type Type;
            public Func<T, object> Provider;
        }

        Dictionary<string, PossibleParameter> possibleParameters = new Dictionary<string, PossibleParameter>();

        public void AddPossibleParameter<TParameter>(string parameterName, Func<T, object> valueProvider)
        {
            possibleParameters.Add(parameterName.ToLower(), new PossibleParameter()
                {
                    Provider                    = valueProvider,
                    Type                    = typeof(TParameter)
                });
        }

        public object[] Generate(MethodInfo method, T data)
        {
            var parameters = new List<object>();
            foreach (var parameterInfo in method.GetParameters())
            {
                parameters.Add(possibleParameters[parameterInfo.Name.ToLower()].Provider(data));
            }
            return parameters.ToArray();
        }

        public void Check(MethodInfo method, ErrorHandler errorHandler)
        {
            var parameters = method.GetParameters();
            if (!parameters.Any())
            {
                errorHandler.OnError(ErrorCode.SelectorMustHaveParameters, FileLocation.None, method.Name, method.DeclaringType.FullName, string.Join(",", possibleParameters.Keys.ToArray()));
            }
            foreach (var parameterInfo in parameters)
            {
                var parameterName = parameterInfo.Name.ToLower();
                if (!possibleParameters.ContainsKey(parameterName))
                {
                    string availableNames = string.Join(", ", (from p in possibleParameters.Keys select "'" + p + "'").ToArray());
                    errorHandler.OnError(ErrorCode.SelectorBadParameterName, FileLocation.None, parameterInfo.Name, method.Name, method.DeclaringType.FullName, availableNames);
                    continue;
                }
                var possibleParameter = possibleParameters[parameterName];
                if (possibleParameter.Type != parameterInfo.ParameterType)
                    errorHandler.OnError(ErrorCode.SelectorBadParameterType, FileLocation.None, parameterInfo.Name, method.Name, method.DeclaringType.FullName, possibleParameter.Type);
            }
        }
    }
}