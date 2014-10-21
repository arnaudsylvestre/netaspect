namespace NetAspect.Sample.MethodWeaving.Problems.Errors
{
    public class ToCheckResultInVoid
    {
        [ToCheckResultInVoidAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }
}