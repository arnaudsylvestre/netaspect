using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetAspect.Weaver.Core.Weaver.Engine.InterceptorParameters;
using NetAspect.Weaver.Core.Weaver.ToSort.Data.Variables;

namespace NetAspect.Weaver.Core.Weaver.ToSort.ILInjector
{
   public static class ParametersIlGenerator
   {
       public static void Generate<T>(IEnumerable<ParameterInfo> parameters, List<Mono.Cecil.Cil.Instruction> instructions, T info, InterceptorParameterPossibilities<T> interceptorParameterPossibilitiesP) where T : VariablesForMethod
       {
         foreach (ParameterInfo parameterInfo in parameters)
         {
            string key_L = parameterInfo.Name.ToLower();
            InterceptorParameterPossibility<T> interceptorParameterPossibilityL = interceptorParameterPossibilitiesP.PossibleParameters.FirstOrDefault(i => i.Name == key_L);
            if (interceptorParameterPossibilityL == null)
               throw new Exception("Parameter unknown : " + parameterInfo.Name);
            interceptorParameterPossibilityL.GenerateIl(parameterInfo, instructions, info);
         }
      }
   }
}
