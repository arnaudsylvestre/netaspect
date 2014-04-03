namespace NetAspect.Sample
{
    internal class Sample
    {
        public static int Beforeinstance;

        public void Before<T>(ref bool instance)
        {
            Before2(instance);
        }

        public void Before2(bool instance)
        {
        }
    }
}