using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.ATrier;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.Detectors.Engine
{
   public static class ParametersIlGenerator
   {
      public static void Generate(IEnumerable<ParameterInfo> parameters, List<Instruction> instructions, IlInjectorAvailableVariables info, InterceptorParameterConfigurations interceptorParameterConfigurations_P)
      {
         foreach (ParameterInfo parameterInfo in parameters)
         {
            string key_L = parameterInfo.Name.ToLower();
            InterceptorParameterConfiguration interceptorParameterConfiguration_L = interceptorParameterConfigurations_P.PossibleParameters.FirstOrDefault(i => i.Name == key_L);
            if (interceptorParameterConfiguration_L == null)
               throw new Exception("Parameter unknown : " + parameterInfo.Name);
            interceptorParameterConfiguration_L.Generator.GenerateIl(parameterInfo, instructions, info);
         }
      }
   }
}
