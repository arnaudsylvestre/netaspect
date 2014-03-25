namespace FluentAspect.Sample.MethodWeaving.Problems.Warnings
{
    public class AspectWithNoInterceptors
    {
        [EmptyAspect]
        public void CheckWithNoInterceptors()
        {
        }
    }
}