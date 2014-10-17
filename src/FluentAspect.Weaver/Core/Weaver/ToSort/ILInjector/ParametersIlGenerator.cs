using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil.Cil;
using NetAspect.Weaver.Core.Weaver.Data;
using NetAspect.Weaver.Core.Weaver.Data.Variables;
using NetAspect.Weaver.Core.Weaver.Detectors.Model;

namespace NetAspect.Weaver.Core.Weaver.ILInjector
{
   public static class ParametersIlGenerator
   {
       public static void Generate<T>(IEnumerable<ParameterInfo> parameters, List<Instruction> instructions, T info, InterceptorParameterConfigurations<T> interceptorParameterConfigurations_P) where T : VariablesForMethod
       {
         foreach (ParameterInfo parameterInfo in parameters)
         {
            string key_L = parameterInfo.Name.ToLower();
            InterceptorParameterConfiguration<T> interceptorParameterConfiguration_L = interceptorParameterConfigurations_P.PossibleParameters.FirstOrDefault(i => i.Name == key_L);
            if (interceptorParameterConfiguration_L == null)
               throw new Exception("Parameter unknown : " + parameterInfo.Name);
            interceptorParameterConfiguration_L.Generator.GenerateIl(parameterInfo, instructions, info);
         }
      }
   }
}
