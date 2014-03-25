namespace FluentAspect.Weaver.Core.Weaver.Checkers
{
    public class InterceptorParametersChecker
    {
        public string ParameterName { get; set; }
        public IInterceptorParameterChecker Checker { get; set; }
    }
}