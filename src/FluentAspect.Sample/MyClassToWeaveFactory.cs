namespace FluentAspect.Sample
{
    public static class MyClassToWeaveFactory
    {
        public static MyClassToWeave Create()
        {
            return new MyClassToWeave();
        }
    }
}