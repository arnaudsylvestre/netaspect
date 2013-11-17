using System;
using FluentAspect.Core.Methods;

namespace FluentAspect.Core
{
    public abstract class FluentAspectDefinition
    {

         protected void WeaveMethodWhichMatches(Func<IMethod> methodMatcher, string functionName)
         {
             
         }
    }
}