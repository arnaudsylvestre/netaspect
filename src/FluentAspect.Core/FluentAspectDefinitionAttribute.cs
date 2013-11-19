using System;
using System.Collections.Generic;
using System.Linq;
using FluentAspect.Core.Methods;

namespace FluentAspect.Core
{
    public abstract class FluentAspectDefinition
    {
       public class MethodMatch
       {
          public Func<IMethod, bool> Matcher { get; set; }

          public string AdviceName { get; set; }
       }

       public List<MethodMatch> methodMatches = new List<MethodMatch>(); 

       public IEnumerable<string> GetAdvices(IMethod method_P)
       {
          return from methodMatch_L in methodMatches where methodMatch_L.Matcher(method_P) select methodMatch_L.AdviceName;

       }

       public abstract void Setup();

         protected void WeaveMethodWhichMatches(Func<IMethod, bool> methodMatcher, string functionName)
         {
            methodMatches.Add(new MethodMatch() { Matcher = methodMatcher, AdviceName = functionName });
         }
    }
}