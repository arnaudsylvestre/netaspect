namespace FluentAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckInstanceWithWrongTypeInAfter
    {
        [ToCheckInstanceWithWrongTypeInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }
}