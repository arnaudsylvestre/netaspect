using System;

namespace FluentAspect.Sample.MethodWeaving.Parameters
{
    public class ToCheckResultInVoid
    {
        [ToCheckResultInVoidAspect]
        public void Check(string parameter1, int parameter2)
        {
            
        }
    }

    public class ToCheckResultInVoidAspectAttribute : Attribute
    {

        private string NetAspectAttributeKind = "MethodWeaving";

        public void After(ref int result)
        {

        }
    }
}