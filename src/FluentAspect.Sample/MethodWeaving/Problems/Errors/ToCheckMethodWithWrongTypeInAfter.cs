using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
{
    public class ToCheckMethodWithWrongTypeInAfter
    {
        [ToCheckMethodWithWrongTypeInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
            
        }
    }

    public class ToCheckMethodWithWrongTypeInAfterAspectAttribute : Attribute
    {

        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(int method)
        {

        }
    }
}