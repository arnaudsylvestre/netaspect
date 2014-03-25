namespace FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersInAfter
    {
        [ToCheckParametersInAfterAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }
}