using System;

namespace NetAspect.Sample
{
    public class CheckLineNumberOnCallAfterAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(int lineNumber, int columnNumber, string filename)
        {
            throw new Exception(lineNumber.ToString() + " : " + columnNumber.ToString() + " : " + filename);
        }
    }
}