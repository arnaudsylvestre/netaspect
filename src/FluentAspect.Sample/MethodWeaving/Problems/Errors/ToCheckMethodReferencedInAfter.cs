namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckMethodReferencedInAfter
    {
        [ToCheckMethodReferencedInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }
}