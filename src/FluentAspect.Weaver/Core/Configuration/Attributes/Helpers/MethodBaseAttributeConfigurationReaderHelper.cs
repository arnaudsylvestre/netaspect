using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAspect.Weaver.Helpers;

namespace FluentAspect.Weaver.Core.Configuration.Attributes.Helpers
{
    public class MethodBaseAttributeConfigurationReaderHelper
    {
         public static void Fill(IEnumerable<MethodBase> methods, List<MethodMatch> matches)
         {
             foreach (var matchingMethod in methods)
             {
                 var info = matchingMethod;
                 matches.Add(new MethodMatch()
                 {
                     InterceptorTypes = matchingMethod.GetNetAspectInterceptors().ToList(),
                     Matcher = m => m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName && ParametersEqual(m, info)
                 });
             }
         }

       private static bool ParametersEqual(IMethod method_P, MethodBase info_P)
       {
          var parameters_L = method_P.Parameters.ToList();
          var parameterInfos_L = info_P.GetParameters();
          if (parameters_L.Count != parameterInfos_L.Length)
             return false;
          for (int i = 0; i < parameters_L.Count; i++)
          {
             if (parameterInfos_L[i].ParameterType.FullName == null)
             {

                if (parameters_L[i].Type.Name != parameterInfos_L[i].ParameterType.Name)
                   return false;
             }
             else
             {
                
             if (parameters_L[i].Type.FullName != parameterInfos_L[i].ParameterType.FullName)
                return false;
             }
             if (parameters_L[i].Name != parameterInfos_L[i].Name)
                return false;
          }
          return true;
       }
    }
}