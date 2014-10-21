namespace NetAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckParametersReferencedInBefore
    {
        [ToCheckParametersReferencedInBeforeAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }
}