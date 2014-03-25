namespace FluentAspect.Sample.MethodWeaving.Parameters.Parameters.Usual
{
    public class ToCheckParametersInBefore
    {
        [ToCheckParametersInBeforeAspect]
        public void Check(string parameter1, int parameter2)
        {
        }
    }
}