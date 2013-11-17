using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAspect.Core;

namespace FluentAspect.Sample
{
    public class AspectMatcherAttribute : Attribute
    {
        private Type t;

        public AspectMatcherAttribute(Type t)
        {
            this.t = t;
        }
    }

    [FluentAspectDefinition]
    public class MyAspects
    {
        [FluentAspectMethod(m => m.Name == "MustRaiseExceptionAfterWeave")]
        public void RaiseException()
        {
            
        }
    }
}
