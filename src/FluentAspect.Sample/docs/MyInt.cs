namespace NetAspect.Sample.docs
{
    public class MyInt
    {
        private readonly int value;

        public MyInt(int value)
        {
            this.value = value;
        }

        public int Value
        {
            get { return value; }
        }

        [Log]
        public int DivideBy(int v)
        {
            return value/v;
        }
    }
}