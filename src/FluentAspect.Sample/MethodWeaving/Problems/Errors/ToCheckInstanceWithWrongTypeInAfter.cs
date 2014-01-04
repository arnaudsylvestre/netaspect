using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
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

        private string NetAspectAttributeKind = "MethodWeaving";

        public void After(int instance)
        {

        }
    }
}