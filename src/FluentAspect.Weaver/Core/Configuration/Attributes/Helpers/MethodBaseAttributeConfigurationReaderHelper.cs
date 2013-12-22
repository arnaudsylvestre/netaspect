using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAspect.Weaver.Core.Fluent.Helpers
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
                     Matcher = m => m.Name == info.Name && m.DeclaringType.FullName == info.DeclaringType.FullName
                 });
             }
         }
    }
}