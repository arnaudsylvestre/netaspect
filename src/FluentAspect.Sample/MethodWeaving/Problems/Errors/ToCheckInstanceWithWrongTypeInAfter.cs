using System;

namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckInstanceWithWrongTypeInAfter
    {
        [ToCheckInstanceWithWrongTypeInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
            
        }
    }

    public class ToCheckInstanceWithWrongTypeInAfterAspectAttribute : Attribute
    {

        public string NetAspectAttributeKind = "MethodWeaving";

        public void After(int instance)
        {

        }
    }
}