using System;
using System.Linq.Expressions;
using FluentAspect.Core.Methods;

namespace FluentAspect.Core
{
    public interface IMethodMatcher
    {
        
    }

    public class FluentAspectMethodAttribute : Attribute
    {
         public FluentAspectMethodAttribute(Type methodMatcher)
         {
             
         }
    }
}