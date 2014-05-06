using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public static class ParametersIlGenerator
    {
        public static void Generate<T>(IEnumerable<ParameterInfo> parameters, List<Instruction> instructions, T info, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P)
        {
            foreach (ParameterInfo parameterInfo in parameters)
            {
                string key_L = parameterInfo.Name.ToLower();
                interceptorParameterConfigurations_P.PossibleParameters.First(i => i.Name == key_L).Generator.GenerateIl(parameterInfo, instructions, info);
            }
        }
    }
}