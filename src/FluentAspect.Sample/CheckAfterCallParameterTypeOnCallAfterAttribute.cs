using System;

namespace FluentAspect.Sample
{
    public class CheckAfterCallParameterTypeOnCallAfterAttribute : Attribute
    {
        public string NetAspectAttributeKind = "CallWeaving";

        public static void AfterCall(string lineNumber, string columnNumber, int fileName, int filePath)
        {
            throw new NotSupportedException();
        }
    }
}