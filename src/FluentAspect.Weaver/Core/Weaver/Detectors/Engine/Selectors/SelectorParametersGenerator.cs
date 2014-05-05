using System;
using System.Collections.Generic;
using System.Reflection;
using NetAspect.Weaver.Core.Errors;

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
            foreach (var parameterInfo in method.GetParameters())
            {
                var possibleParameter = possibleParameters[parameterInfo.Name.ToLower()];
                if (possibleParameter.Type != parameterInfo.ParameterType)
                    errorHandler.OnError("The parameter {0} in the method {1} of the aspect {2} is expected to be {3}", parameterInfo.Name, method.Name, method.DeclaringType.FullName, possibleParameter.Type);
            }
        }
    }
}