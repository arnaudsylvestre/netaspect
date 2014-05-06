using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Errors;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;
using NetAspect.Weaver.Core.Weaver.WeavingBuilders.Method;

namespace NetAspect.Weaver.Core.Weaver.Generators
{
    public static class ParametersIlGenerator
    {
       public static void Generate(IEnumerable<ParameterInfo> parameters, List<Instruction> instructions, IlInjectorAvailableVariables info, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
        {
            foreach (ParameterInfo parameterInfo in parameters)
            {
                string key_L = parameterInfo.Name.ToLower();
                interceptorParameterConfigurations_P.PossibleParameters.First(i => i.Name == key_L).Generator.GenerateIl(parameterInfo, instructions, info);
            }
        }
    }
}