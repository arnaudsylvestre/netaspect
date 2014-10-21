namespace NetAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckInstanceReferencedInAfter
    {
        [ToCheckInstanceReferencedInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }
}