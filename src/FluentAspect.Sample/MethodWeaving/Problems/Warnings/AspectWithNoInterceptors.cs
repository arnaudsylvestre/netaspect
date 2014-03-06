using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentAspect.Sample.MethodWeaving.Problems.Warnings
{
    public class AspectWithNoInterceptors
    {
        [EmptyAspect]
        public void CheckWithNoInterceptors()
        {
            
        }
    }

    public class EmptyAspectAttribute : Attribute
    {
        public string NetAspectAttributeKind = "MethodWeaving";
    }
}
