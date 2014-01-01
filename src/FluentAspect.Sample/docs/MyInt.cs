namespace FluentAspect.Sample.docs
{
    public class MyInt
    {
        int value;

        public MyInt(int value)
        {
            this.value = value;
        }

        public int Value
        {
            get { return value; }
        }

        public int DivideBy(int v)
        {
            return value / v;
        }
    }
}