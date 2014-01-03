namespace FluentAspect.Sample.docs
{
    public class Math
    {
        public int Divide(int dividend, int divisor)
        {
            var myInt = new MyInt(dividend);
            return myInt.DivideBy(divisor);
        }
    }
}