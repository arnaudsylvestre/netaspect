namespace FluentAspect.Sample
{
    public class MyClassToWeave
    {
        public string MustRaiseExceptionAfterWeave()
        {
            return "NotWeaved";
        }
    }
}