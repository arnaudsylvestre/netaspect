using System;
using System.Reflection;

namespace FluentAspect.Sample.MethodWeaving.Parameters
{
    public class ToCheckMethodReferencedInAfter
    {
        [ToCheckMethodReferencedInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
            
        }
    }

    public class ToCheckMethodReferencedInAfterAspectAttribute : Attribute
    {

        private string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref MethodInfo method)
        {

        }
    }
}